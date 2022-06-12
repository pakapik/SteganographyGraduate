using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SteganographyGraduate
{
    public partial class StegoForm : Form
    {
        public StegoForm(UserControl encryptView, UserControl decryptView, UserControl checkView)
        {
            InitializeComponent();

            tabPage_encrypt.Controls.Add(encryptView);
            tabPage_decrypt.Controls.Add(decryptView);
            tabPage_check.Controls.Add(checkView);
        }
    }
}
