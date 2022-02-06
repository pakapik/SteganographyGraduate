using SteganographyGraduate.Decrypters;
using SteganographyGraduate.Encrypters;
using SteganographyGraduate.Views;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyGraduate.Presenters
{
    public class DecryptPresenter //: IPresenter
    {
        private readonly IView _view;
        private readonly IService _service;

        public OpenFileDialog ImageFileDialog { get; set; }
        public OpenFileDialog TxtFileDialog { get; set; }
        public SaveFileDialog ResultFileDialog { get; set; }

        public DecryptPresenter(IView view, IService service)
        {
            _view = view;
            _service = service;

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

            using (var decrypter = IoCContainer.Instance.Create<IDecrypter>(GetDecrypterName(), new object[] { _view.ImagePath, _view.Key }))
            {
                var decryptMessage = await Task.Run(() => decrypter.Decrypt());

                await WriteToFileAsync(decryptMessage);
            }
        }

        private async Task WriteToFileAsync(IList<byte> decryptMessage)
        {
            if (!CheckMessageLength(decryptMessage))
            {
                return;
            }

            using (var file = File.Open(ResultFileDialog.FileName, FileMode.OpenOrCreate))
            {
                await file.WriteAsync(decryptMessage.ToArray(), 0, decryptMessage.Count);
            }
        }

        private string GetDecrypterName()
        {
            var separatorIndex = _view.GetActiveRadioButtonName.IndexOf('_');

            return $"{_view.GetActiveRadioButtonName.Substring(separatorIndex + 1)}Decrypter";
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

        private bool CheckPath()
        {
            if (string.IsNullOrEmpty(ResultFileDialog.FileName))
            {
                _view.ShowError("Укажите путь к файлу, куда будет сохранена информация!");
                return false;
            }

            return true;
        }

        public void OnImageOpenFileDialog()
        {
            if (ImageFileDialog.ShowDialog() == DialogResult.OK)
            {
                _view.ImagePath = ImageFileDialog.FileName;
            }
        }

        public void OnTxtOpenFileDialog()
        {
            if (ResultFileDialog.ShowDialog() == DialogResult.OK)
            {
                _view.TxtPath = ResultFileDialog.FileName;
            }
        }

        private void CustomizeUI()
        {
            _view.ButtonImageOpenFileDialogDescription = "Указать";
            _view.ButtonTxtOpenFileDialogDescription = "Указать";
            _view.ButtonMainButtonDescription = "Извлечь";

            _view.ImagePathDescription = "Укажите путь к стегоконтейнеру:";
            _view.TxtPathDescription = "Укажите путь к файлу, куда будет сохранена информация:";
            _view.KeyDescription = "Введите ключ:";

            _view.FirstRBtnDescription = "Простое изъятие информации";
            _view.SecondRBtnDescription = "Изъятие информации с рандомайзером";
            _view.ThridBtnDescription = "LSB-метод";
            _view.FourthRBtnDescription = "СТАБ РУНГЕ КУТ";
        }
    }
}
