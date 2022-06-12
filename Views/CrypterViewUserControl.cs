using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyGraduate.Views
{
    public partial class CrypterViewUserControl : UserControl, IView
    {
        private readonly string _name;

        #region RadioButtons

        public RadioButton FirstRBtn
        {
            get => rBtn_RLB;
            set => rBtn_RLB = value;
        }

        public RadioButton SecondRBtn
        {
            get => rBtn_LSB;
            set => rBtn_LSB = value;
        }

        public RadioButton ThridBtn
        {
            get => rBtn_CJB;
            set => rBtn_CJB = value;
        }

        public RadioButton FourthRBtn
        {
            get => rBtn_DCT;
            set => rBtn_DCT = value;
        }

        #endregion

        #region TextBoxes

        public TextBox Tb_ImagePath
        {
            get => tb_imgPath;
            set => tb_imgPath = value;
        }

        public TextBox Tb_TxtPath
        {
            get => tb_txtPath;
            set => tb_txtPath = value;
        }

        public TextBox Tb_Key
        {
            get => tb_key;
            set => tb_key = value;
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

        #region Buttons 

        public Button ImageOpenFileDialogButton
        {
            get => btn_openImgFileDialog;
            set => btn_openImgFileDialog = value;
        }

        public Button TxtOpenFileDialogButton
        {
            get => btn_openTxtFileDialog;
            set => btn_openTxtFileDialog = value;
        }

        public Button MainActionButton
        {
            get => btn_mainAction;
            set => btn_mainAction = value;
        }

        #endregion

        private string _activeRBtnName;
        public string GetActiveRadioButtonName => _activeRBtnName;

        #region Button-click event

        public event Action OnImageOpenFileDialog;
        public event Action OnTxtOpenFileDialog;
        public event Func<Task> OnMainButtonClickAsync;

        #endregion

        public CrypterViewUserControl(string name)
        {
            InitializeComponent();
            _activeRBtnName = rBtn_RLB.Name;
            _name = name;
        }

        public void ShowError(string message) => MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void ShowInfo(string message) => MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            if (rBtn_RLB.Checked)
            {
                _activeRBtnName = rBtn_RLB.Name;
            }
        }

        private void rBtn_LSB_CheckedChanged(object sender, EventArgs e)
        {
            if(rBtn_LSB.Checked)
            {
                _activeRBtnName = rBtn_LSB.Name;
            }
        }

        private void rBtn_CJB_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtn_CJB.Checked)
            {
                _activeRBtnName = rBtn_CJB.Name;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (rBtn_DCT.Checked)
            {
                _activeRBtnName = rBtn_DCT.Name;
            }
        }
        #endregion

    }
}
