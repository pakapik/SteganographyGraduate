using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MathNet.Numerics.Distributions;

using SteganographyGraduate.Models.Analyzers;
using SteganographyGraduate.Models.Decrypters;
using SteganographyGraduate.Models.Encrypters;
using SteganographyGraduate.Views;

namespace SteganographyGraduate.Presenters
{
    public class EncryptPresenter : IPresenter
    {
        private readonly IView _view;

        public OpenFileDialog ImageFileDialog { get; set; }
        public OpenFileDialog TxtFileDialog { get; set; }
        public SaveFileDialog ResultFileDialog { get; set; }

        public EncryptPresenter(IView view)
        {
            _view = view;

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
                Filter = "PNG Image|*.png|JPG Image|*.jpg"
            };

            #endregion

            _view.OnImageOpenFileDialog += OnImageOpenFileDialog;
            _view.OnTxtOpenFileDialog += OnTxtOpenFileDialog;
            _view.OnMainButtonClickAsync += OnMainButtonClick;

            //  _view.OnMainButtonClickAsync += test_OnMainButtonClickCreateDifference;
            //  _view.OnMainButtonClickAsync += test_OnMainButtonClickCountGreen;
            //  _view.OnMainButtonClickAsync += test_OnMainButtonClickCreateLSB;
        }

        #region test

        public async Task test_OnMainButtonClickCountGreen()
        {
            var img = new UnsafeBitmap(@"D:\Sanyok\яаэро\fqw_stego\sample\rs_check\res1.png");
            var pixel = new Pixel();
            var rows = img.Width;
            var columns = img.Height;
            var lst = new List<Pixel>(rows * columns);
            var greens = 0;
            img.LockBitmap();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    lst.Add(img[i, j]);
                    if (img[i, j] != pixel)
                    {
                        greens++;
                    }
                }
            }
            var colors = lst.GroupBy(x => x).ToList();
            var blacks = rows * columns - greens;
            var percents = (double)greens / blacks;
            img.UnlockBitmap();
        }

        public async Task test_OnMainButtonClickCreateDifference()
        {
            var emptyImg = new UnsafeBitmap(@"D:\Sanyok\яаэро\fqw_stego\sample1\zwave_cut.jpg");
            var fullImg = new UnsafeBitmap(@"D:\Sanyok\яаэро\fqw_stego\sample1\dct_not_my.jpg");

            var rows = fullImg.Width;
            var columns = fullImg.Height;
            var result = new UnsafeBitmap(new Bitmap(rows, columns));
            fullImg.LockBitmap();
            emptyImg.LockBitmap();
            result.LockBitmap();
            var task = await Task.Run(() =>
            {
                var fullCount = 0;
                var emptyCount = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (fullImg[i, j] != emptyImg[i, j])
                        {
                            var p = emptyImg[i, j];
                            p.G = 0;
                            p.R = 0;
                            p.B = 0;
                            result[i, j] = p;

                            //p.G = 255; //забавные контуры получаются
                            //result[i, j] = p;

                            fullCount++;
                        }
                        else
                        {
                            var p = emptyImg[i, j];
                            p.G = 255;
                            p.R = 0;
                            p.B = 0;
                            result[i, j] = p;
                        }
                        emptyCount++;
                    }
                }
                emptyCount -= fullCount;
                var count = (rows * columns) / 3;
                var filling = fullCount / (count / 100.0);
                return false;
            });

            fullImg.UnlockBitmap();
            emptyImg.UnlockBitmap();
            result.UnlockBitmap();
            Save(result.Bitmap);
        }

        public async Task test_OnMainButtonClickCreateLSB()
        {
            var img = new UnsafeBitmap(@"D:\Sanyok\Новая папка (3)\ываыва.png");

            var columns = img.Width;
            var rows = img.Height;
            img.LockBitmap();
            var task = await Task.Run(() =>
            {
                for (int x = 0; x < rows; x++)
                {
                    for (int y = 0; y < columns; y++)
                    {
                        var p = img[y, x];
                        var r = (byte)((p.R & 0b0000_0001) == 1 ? 255 : 0);
                        var g = (byte)((p.G & 0b0000_0001) == 1 ? 255 : 0);
                        var b = (byte)((p.B & 0b0000_0001) == 1 ? 255 : 0);
                        img[y, x] = new Pixel(r, g ,b);

                    }
                }
                return false;
            });

            img.UnlockBitmap();

            Save(img.Bitmap);
        }

        #endregion

        public async Task OnMainButtonClick()
        {
            if (!CheckPath())
            {
                return;
            }

            using (var encrypter = IoCContainer.Instance.Create<IEncrypter>(GetEncrypterName(), 
                                                                            new object[] { _view.Tb_ImagePath.Text, _view.Tb_TxtPath.Text, _view.Tb_Key.Text }))
            {
                if(!encrypter.CheckCapacity())
                {
                    _view.ShowError("Контейнер слишком мал для внедряемого сообщения!");
                    return;

                }

                var bitmap = await Task.Run(() => encrypter.Encrypt());
                Save(bitmap);
            }
        }

        private void Save(Bitmap bitmap)
        {
            if (ResultFileDialog.ShowDialog() == DialogResult.OK)
            {
                var ext = Path.GetExtension(ResultFileDialog.FileName);
                var imgFormat = ImageFormat.Png;

                if(ext == ".jpg")
                {
                    SaveAsJpeg(bitmap);
                    return;
                }

                bitmap.Save(ResultFileDialog.FileName, imgFormat);
            }
        }

        private void SaveAsJpeg(Bitmap bitmap)
        {
            var encoder = System.Drawing.Imaging.Encoder.Quality;
            var jpgEncoder = ImageCodecInfo.GetImageEncoders()
                                           .FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);

            var encoderParameters = new EncoderParameters(1);
            // 100L - минимальное сжатие. [0L, 100L];
            encoderParameters.Param[0] = new EncoderParameter(encoder, 100L);

            bitmap.Save(ResultFileDialog.FileName, jpgEncoder, encoderParameters);
        }

        private string GetEncrypterName()
        {
            var separatorIndex = _view.GetActiveRadioButtonName.IndexOf('_');

            return $"{_view.GetActiveRadioButtonName.Substring(separatorIndex + 1)}Encrypter";
        }

        private bool CheckPath()
        {
            if (string.IsNullOrEmpty(ImageFileDialog.FileName))
            {
                _view.ShowError("Укажите путь  к изображению!");
                return false;
            }

            if (string.IsNullOrEmpty(TxtFileDialog.FileName))
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
                _view.Tb_ImagePath.Text = ImageFileDialog.FileName;
            }
        }

        public void OnTxtOpenFileDialog()
        {
            if (TxtFileDialog.ShowDialog() == DialogResult.OK)
            {
                _view.Tb_TxtPath.Text = TxtFileDialog.FileName;
            }
        }

        private void CustomizeUI()
        {
            _view.ImageOpenFileDialogButton.Text = "Указать";
            _view.TxtOpenFileDialogButton.Text = "Указать";
            _view.MainActionButton.Text = "Внедрить";

            _view.ImagePathDescription = "Укажите путь к изображению:";
            _view.TxtPathDescription = "Укажите путь к файлу с сообщением:";
            _view.KeyDescription = "Введите ключ:";

            _view.FirstRBtn.Text = "Метод замены наименьших значащих бит";
            _view.SecondRBtn.Text = "Модификация метода замены наименьших значащих бит";
            _view.ThridBtn.Text = "Метод Куттера-Джордана-Боссена";
            _view.FourthRBtn.Text = "Метод Коха и Жао";
        }
    }
}
