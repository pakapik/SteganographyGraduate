using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyGraduate.Views
{
    public interface IView
    {
        #region Labels

        string ImagePathDescription { get; set; }
        string TxtPathDescription { get; set; }
        string KeyDescription { get; set; }

        #endregion

        #region Description of buttons 

        string ButtonImageOpenFileDialogDescription { get; set; }
        string ButtonTxtOpenFileDialogDescription { get; set; }
        string ButtonMainButtonDescription { get; set; }

        #endregion

        #region TextBoxes

        string ImagePath { get; set; }
        string TxtPath { get; set; }
        string Key { get; set; }

        #endregion

        #region Button-click event

        event Action OnImageOpenFileDialog;
        event Action OnTxtOpenFileDialog;
        event Func<Task> OnMainButtonClickAsync;

        void ShowError(string message);

        #endregion

        #region RadioButton descriptions

        string FirstRBtnDescription { get; set; }
        string SecondRBtnDescription { get; set; }
        string ThridBtnDescription { get; set; }
        string FourthRBtnDescription { get; set; }

        #endregion

        #region RadioButton names

        string GetActiveRadioButtonName { get; }

        #endregion
    }
}
