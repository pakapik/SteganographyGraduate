using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteganographyGraduate.Presenters;

namespace SteganographyGraduate
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var encryptView = new MainViewUserControl("encryptView");
            var decryptView = new MainViewUserControl("decryptView");
            var checkView = new MainViewUserControl("checkView");

            var form = new StegoForm(encryptView, decryptView, checkView);
            var encryptPresenter = new EncryptPresenter(encryptView, null);
            var decryptPresenter = new DecryptPresenter(decryptView, null);
            Application.Run(form);
        }
    }
}
