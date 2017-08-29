using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCF.Excel.UI
{
    public static class UI
    {
        public static Tuple<bool, bool, bool, bool, DateTime, DateTime> GetQueryParameters(bool withZone = false)
        {
            QueryParForm f = new QueryParForm(DateTime.Now.AddDays(-1.0), DateTime.Now, withZone);
            f.ShowDialog();
            return new Tuple<bool, bool, bool, bool, DateTime, DateTime>(
                f.DialogResult == System.Windows.Forms.DialogResult.OK,
                f.WithUG, f.WithOPD, f.WithWinder,
                f.TimeFrom, f.TimeTo);
        }
        public static void LongOpBegin()
        {
        }
    }
}
