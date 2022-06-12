using SteganographyGraduate.Models.Analyzers;
using SteganographyGraduate.Models.Encrypters;
using SteganographyGraduate.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyGraduate.Presenters
{
    public class AnylizePresenter : IPresenter
    {
        private readonly IView _view;

        public OpenFileDialog ImageFileDialog { get; set; }
        public OpenFileDialog TxtFileDialog { get; set; }
        public SaveFileDialog ResultFileDialog { get; set; }

        public AnylizePresenter(IView view)
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
            _view.OnMainButtonClickAsync += OnMainButtonClickAsync;
        }

        public async Task OnMainButtonClickAsync()
        {
            if(!CheckPath())
            {
                return;
            }

            using (var analyzer = IoCContainer.Instance.Create<IAnalyzer>(GetAnalyzerName(), new object[] { _view.Tb_ImagePath.Text }))
            {
                var result = await Task.Run(() => analyzer.Analyze());

                _view.ShowInfo(analyzer.GetResult());
            }
        }

        private bool CheckPath()
        {
            if (string.IsNullOrEmpty(ImageFileDialog.FileName))
            {
                _view.ShowError("Укажите путь к стегоконтейнеру!");
                return false;
            }

            return true;
        }

        private string GetAnalyzerName()
        {
            var separatorIndex = _view.GetActiveRadioButtonName.IndexOf('_');

            return $"{_view.GetActiveRadioButtonName.Substring(separatorIndex + 1)}Analyzer";
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

        private void CustomizeUI()
        {
            _view.FirstRBtn.Name = "rBtn_ChiSquare";
            _view.SecondRBtn.Name = "rBtn_RS";

            _view.TxtOpenFileDialogButton.Visible = false;
            _view.Tb_TxtPath.Visible = false;
            _view.Tb_Key.Visible = false;

            _view.TxtOpenFileDialogButton.Text = string.Empty;
            _view.TxtPathDescription = string.Empty;
            _view.KeyDescription = string.Empty;

            _view.ImageOpenFileDialogButton.Text = "Указать";
            _view.MainActionButton.Text = "Проверить";
           
            _view.ImagePathDescription = "Укажите путь к стегоконтейнеру:";

            _view.FirstRBtn.Text = "Гистограммный анализ";
            _view.SecondRBtn.Text = "Регулярно-сингулярный анализ";

            _view.ThridBtn.Visible = false;
            _view.FourthRBtn.Visible = false;
        }
    }
}
