using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteganographyGraduate.Views;

namespace SteganographyGraduate
{
    public partial class MainViewUserControl : UserControl, IView
    {
        private readonly string _name;

        #region TextBoxes

        public string ImagePath
        {
            get => tb_imgPath.Text;
            set => tb_imgPath.Text = value;
        }

        public string TxtPath
        {
            get => tb_txtPath.Text;
            set => tb_txtPath.Text = value;
        }

        public string Key
        {
            get => tb_key.Text;
            set => tb_key.Text = value;
        }

        #endregion

        #region Labels

        public string ImagePathDescription
        {
            get => lbl_imgPathDescription.Text;
            set => lbl_imgPathDescription.Text = value;
        }

        public string TxtPathDescription
        {
            get => lbl_txtPathDescription.Text;
            set => lbl_txtPathDescription.Text = value;
        }

        public string KeyDescription
        {
            get => lbl_keyDescription.Text;
            set => lbl_keyDescription.Text = value;
        }

        #endregion

        #region Description of buttons 

        public string ButtonImageOpenFileDialogDescription
        {
            get => btn_openImgFileDialog.Text;
            set => btn_openImgFileDialog.Text = value;
        }

        public string ButtonTxtOpenFileDialogDescription
        {
            get => btn_openTxtFileDialog.Text;
            set => btn_openTxtFileDialog.Text = value;
        }

        public string ButtonMainButtonDescription
        {
            get => btn_mainAction.Text;
            set => btn_mainAction.Text = value;
        }

        #endregion

        #region RadioButton descriptions

        public string FirstRBtnDescription
        {
            get => rBtn_Simple.Text;
            set => rBtn_Simple.Text = value;
        }

        public string SecondRBtnDescription
        {
            get => rBtn_SimpleRandom.Text;
            set => rBtn_SimpleRandom.Text = value;
        }

        public string ThridBtnDescription
        {
            get => rBtn_LSB.Text;
            set => rBtn_LSB.Text = value;
        }

        public string FourthRBtnDescription
        {
            get => radioButton4.Text;
            set => radioButton4.Text = value;
        }

        #endregion

        private string _activeRBtnName;
        private Func<string> _getActiveRadioButtonName;
        public string GetActiveRadioButtonName =>_activeRBtnName;

        #region Button-click event

        public event Action OnImageOpenFileDialog;
        public event Action OnTxtOpenFileDialog;
        public event Func<Task> OnMainButtonClickAsync;

        #endregion

        public MainViewUserControl(string name)
        {
            InitializeComponent();
            _activeRBtnName = rBtn_Simple.Name;
            _getActiveRadioButtonName = () => Controls.OfType<RadioButton>().First(x => x.Checked).Name;
            _name = name;
        }

        public void ShowError(string message) => MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public override string ToString() => _name;

        private void btn_openImgFileDialog_Click(object sender, EventArgs e) => OnImageOpenFileDialog?.Invoke();

        private void btn_openTxtFileDialog_Click(object sender, EventArgs e) => OnTxtOpenFileDialog?.Invoke();

        private void btn_mainAction_Click(object sender, EventArgs e) => OnMainButtonClickAsync?.Invoke();

        #region RadioButton Checked changes

        // Этот код добавил прям только что, потому что по какой-то причине 
        // GetActiveRadioButtonName падает с InvalidOperationException,
        // не помогает и делегат _getActiveRadioButtonName, вызываемый при вызове GetActiveRadioButtonName
        // с другими свойствами таких проблем нет.
        // Не очень понимаю, откуда вообще падает исключение, т.к. обращение в презентере идет 
        // в том же потоке, в котором находится форма.

        private void rBtn_Simple_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtn_Simple.Checked)
            {
                _activeRBtnName = rBtn_Simple.Name;
            }
        }

        private void rBtn_SimpleRandom_CheckedChanged(object sender, EventArgs e)
        {
            if(rBtn_SimpleRandom.Checked)
            {
                _activeRBtnName = rBtn_SimpleRandom.Name;
            }
        }

        private void rBtn_3_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtn_LSB.Checked)
            {
                _activeRBtnName = rBtn_LSB.Name;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                _activeRBtnName = radioButton4.Name;
            }
        }
        #endregion

    }
}
