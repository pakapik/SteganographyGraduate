using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteganographyGraduate.Presenters
{
    public interface IPresenter
    {
        OpenFileDialog ImageFileDialog { get; set; }
        OpenFileDialog TxtFileDialog { get; set; }
        SaveFileDialog ResultFileDialog { get; set; }
    }
}
