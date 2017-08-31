using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCF.Excel.Forms
{
    public partial class QueryParForm : Form
    {
        internal QueryParForm()
        {
            InitializeComponent();
        }

        internal QueryParForm(DateTime tFrom, DateTime tTo, bool withZone) : this()
        {
            TimeFrom = tFrom;
            TimeTo = tTo;
            checkBoxUG.Enabled = checkBoxOPD.Enabled = checkBoxWinder.Enabled = withZone;
        }

        internal DateTime TimeFrom {
            get {
                return dateTimePickerFrom.Value;
            }
            set {
                dateTimePickerFrom.Value = value;
            }
        }
        internal DateTime TimeTo {
            get {
                return dateTimePickerTo.Value;
            }
            set {
                dateTimePickerTo.Value = value;
            }
        }
        internal bool WithUG { get { return checkBoxUG.Checked; } }
        internal bool WithOPD { get { return checkBoxOPD.Checked; } }
        internal bool WithWinder { get { return checkBoxWinder.Checked; } }
    }
}
