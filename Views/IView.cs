using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyGraduate.Views
{
    public interface IView
    {
        #region Labels

        string ImagePathDescription { get; set; }
        string TxtPathDescription { get; set; }
        string KeyDescription { get; set; }

        #endregion

        #region Buttons 

        Button ImageOpenFileDialogButton { get; set; }
        Button TxtOpenFileDialogButton { get; set; }
        Button MainActionButton { get; set; }

        #endregion

        #region TextBoxes

        TextBox Tb_ImagePath { get; set; }
        TextBox Tb_TxtPath { get; set; }
        TextBox Tb_Key { get; set; }

        #endregion

        #region Button-click event

        event Action OnImageOpenFileDialog;
        event Action OnTxtOpenFileDialog;
        event Func<Task> OnMainButtonClickAsync;

        void ShowError(string message);

        void ShowInfo(string message);

        #endregion

        #region RadioButtons

        RadioButton FirstRBtn { get; set; }
        RadioButton SecondRBtn { get; set; }
        RadioButton ThridBtn { get; set; }
        RadioButton FourthRBtn { get; set; }

        #endregion

        #region RadioButton names

        string GetActiveRadioButtonName { get; }

        #endregion
    }
}
