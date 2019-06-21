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
    public partial class WRRQueryParForm : Form
    {
        internal WRRQueryParForm()
        {
            InitializeComponent();
        }

        internal WRRQueryParForm(DateTime tFrom, DateTime tTo) : this()
        {
            TimeFrom = tFrom;
            TimeTo = tTo;
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
        internal bool Downtimes { get { return downtimeRadioButton.Checked; } }
        internal bool Cycles { get { return cyclesRadioButton.Checked; } }
    }
}
