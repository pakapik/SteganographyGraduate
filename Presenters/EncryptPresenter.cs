using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteganographyGraduate.Decrypters;
using SteganographyGraduate.Encrypters;
using SteganographyGraduate.Views;

namespace SteganographyGraduate.Presenters
{
    public class EncryptPresenter : IPresenter
    {
        private readonly IView _view;
        private readonly IService _service;

        public OpenFileDialog ImageFileDialog { get; set; }
        public OpenFileDialog TxtFileDialog { get; set; }
        public SaveFileDialog ResultFileDialog { get; set; }

        public EncryptPresenter(IView view, IService service)
        {
            _view = view;
            _service = service;

            CustomizeUI();

            #region FileDialog

            ImageFileDialog = new OpenFileDialog()
            {
                Filter = "Images (*.jpg;*.png)|*.jpg;*.png"
            };

            TxtFileDialog = new OpenFileDialog()
            {
                Filter = "txt files (*.txt)|*.txt"
            };

            ResultFileDialog = new SaveFileDialog()
            {
                Filter = "Images (*.jpg;*.png)|*.jpg;*.png"
            };

            #endregion

            _view.OnImageOpenFileDialog += OnImageOpenFileDialog;
            _view.OnTxtOpenFileDialog += OnTxtOpenFileDialog;
            _view.OnMainButtonClickAsync += OnMainButtonClick;
        }

        public async Task OnMainButtonClick()
        {
            if (!CheckPathToSource())
            {
                return;
            }

            using (var encrypter = IoCContainer.Instance.Create<IEncrypter>(GetEncrypterName(), new object[] { _view.ImagePath, _view.TxtPath, _view.Key }))
            {
                var bitmap = await Task.Run(() => encrypter.Encrypt());

                Save(bitmap);
            }
        }

        private void Save(Bitmap bitmap)
        {
            if (ResultFileDialog.ShowDialog() == DialogResult.OK)
            {
                // TODO: JPEG сохраняется с сжатием
                bitmap.Save(ResultFileDialog.FileName, ImageFormat.Png);
            }
        }

        private string GetEncrypterName()
        {
            var separatorIndex = _view.GetActiveRadioButtonName.IndexOf('_');

            return $"{_view.GetActiveRadioButtonName.Substring(separatorIndex + 1)}Encrypter";
        }

        private bool CheckPathToSource()
        {
            if (string.IsNullOrEmpty(ImageFileDialog.FileName))
            {
                _view.ShowError("Укажите путь к стегоконтейнеру!");
                return false;
            }

            if (string.IsNullOrEmpty(ImageFileDialog.FileName))
            {
                _view.ShowError("Укажите путь к файлу с сообщением!");
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
            if (TxtFileDialog.ShowDialog() == DialogResult.OK)
            {
                _view.TxtPath = TxtFileDialog.FileName;
            }
        }

        private void CustomizeUI()
        {
            _view.ButtonImageOpenFileDialogDescription = "Указать";
            _view.ButtonTxtOpenFileDialogDescription = "Указать";
            _view.ButtonMainButtonDescription = "Внедрить";

            _view.ImagePathDescription = "Укажите путь к изображению:";
            _view.TxtPathDescription = "Укажите путь к файлу с сообщением:";
            _view.KeyDescription = "Введите ключ:";

            _view.FirstRBtnDescription = "Простое внедрение информации";
            _view.SecondRBtnDescription = "Внедрение информации с рандомайзером";
            _view.ThridBtnDescription = "LSB-метод";
            _view.FourthRBtnDescription = "СТАБ РУНГЕ КУТ";
        }
    }
}
