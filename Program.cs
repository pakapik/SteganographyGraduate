using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteganographyGraduate.Presenters;
using SteganographyGraduate.Views;

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

            var encryptView = new CrypterViewUserControl("encryptView");
            var decryptView = new CrypterViewUserControl("decryptView");
            var analyzeView = new AnalyzerViewUserControl("analyzeView");

            var form = new StegoForm(encryptView, decryptView, analyzeView);
            var encryptPresenter = new EncryptPresenter(encryptView);
            var decryptPresenter = new DecryptPresenter(decryptView);
            var analyzePresenter = new AnylizePresenter(analyzeView);

            Application.Run(form);

            var arr1 = new int[] { 1, 3, 4, 5, 23, 35, 4, 34, 62, 5, 4, 61, 5 };
            var arr2 = new int[arr1.Length / 2];
            for (var i = 0; i < arr2.Length; i += 2)
            {
                arr2[i] = arr1[i * 2];
            }
        }
    }
}
