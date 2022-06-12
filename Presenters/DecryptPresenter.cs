using SteganographyGraduate.Models.Decrypters;
using SteganographyGraduate.Views;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyGraduate.Presenters
{
    public class DecryptPresenter : IPresenter
    {
        private readonly IView _view;

        public OpenFileDialog ImageFileDialog { get; set; }
        public OpenFileDialog TxtFileDialog { get; set; }
        public SaveFileDialog ResultFileDialog { get; set; }

        public DecryptPresenter(IView view)
        {
            _view = view;
            CustomizeUI();
           
            #region FileDialog

            ImageFileDialog = new OpenFileDialog()
            {
                Filter = "Images (*.jpg;*.png)|*.jpg;*.png"
            };

            ResultFileDialog = new SaveFileDialog()
            {
                Filter = "txt files (*.txt)|*.txt"
            };

            #endregion

            _view.OnImageOpenFileDialog += OnImageOpenFileDialog;
            _view.OnTxtOpenFileDialog += OnTxtOpenFileDialog;
            _view.OnMainButtonClickAsync += OnMainButtonClickAsync;
        }

        public async Task OnMainButtonClickAsync()
        {
            if (!CheckPath())
            {
                return;
            }

            using (var decrypter = IoCContainer.Instance.Create<IDecrypter>(GetDecrypterName(),
                                                                            new object[] { _view.Tb_ImagePath.Text, _view.Tb_Key.Text }))
            {
                var decryptMessage = await Task.Run(() => decrypter.Decrypt());

                await WriteToFileAsync(decryptMessage);
            }
        }

        private bool CheckPath()
        {
            if (string.IsNullOrEmpty(ImageFileDialog.FileName))
            {
                _view.ShowError("Укажите путь к стегоконтейнеру!");
                return false;
            }

            if (string.IsNullOrEmpty(ResultFileDialog.FileName))
            {
                _view.ShowError("Укажите путь к файлу, куда будет сохранена информация!");
                return false;
            }

            return true;
        }

        private string GetDecrypterName()
        {
            var separatorIndex = _view.GetActiveRadioButtonName.IndexOf('_');

            return $"{_view.GetActiveRadioButtonName.Substring(separatorIndex + 1)}Decrypter";
        }

        private async Task WriteToFileAsync(IList<byte> decryptMessage)
        {
            if (!CheckMessageLength(decryptMessage))
            {
                return;
            }

            using (var file = File.Open(ResultFileDialog.FileName, FileMode.OpenOrCreate))
            {
                var data = decryptMessage is byte[] bytes
                         ? bytes
                         : decryptMessage.ToArray();

                await file.WriteAsync(data, 0, data.Length);
            }
        }

        private bool CheckMessageLength(IList<byte> decryptMessage)
        {
            if (decryptMessage.Count == 0)
            {
                _view.ShowError("В контейнере не найдена информация!");
                return false;
            }

            return true;
        }

        public void OnImageOpenFileDialog()
        {
            if (ImageFileDialog.ShowDialog() == DialogResult.OK)
            {
                _view.Tb_ImagePath.Text = ImageFileDialog.FileName;
            }
        }

        public void OnTxtOpenFileDialog()
        {
            if (ResultFileDialog.ShowDialog() == DialogResult.OK)
            {
                _view.Tb_TxtPath.Text = ResultFileDialog.FileName;
            }
        }

        private void CustomizeUI()
        {
            _view.ImageOpenFileDialogButton.Text = "Указать";
            _view.TxtOpenFileDialogButton.Text = "Указать";
            _view.MainActionButton.Text = "Извлечь";

            _view.ImagePathDescription = "Укажите путь к стегоконтейнеру:";
            _view.TxtPathDescription = "Укажите путь к файлу, куда будет сохранена информация:";
            _view.KeyDescription = "Введите ключ:";

            _view.FirstRBtn.Text = "Метод замены наименьших значащих бит";
            _view.SecondRBtn.Text = "Модификация метода замены наименьших значащих бит";
            _view.ThridBtn.Text = "Метод Куттера-Джордана-Боссена";
            _view.FourthRBtn.Text = "Метод Коха и Жао";
        }
    }
}
