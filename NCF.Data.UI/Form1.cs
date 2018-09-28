using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NCF.Data.UI {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        public static object[] SelectFromList(IEnumerable list) {
            Form1 f = new Form1();
            f.checkedListBox1.Items.AddRange(list.Cast<object>().ToArray());
            f.ShowDialog();
            return f.checkedListBox1.CheckedItems.Cast<object>().ToArray();
        }
    }
}
