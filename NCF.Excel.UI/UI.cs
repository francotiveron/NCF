using System;
using NCF.Excel.Forms;

namespace NCF.Excel
{
    public static class UI
    {
        public static Tuple<bool, bool, bool, bool, DateTime, DateTime> GetCAHQueryParameters(bool withZone = false)
        {
            CAHQueryParForm f = new CAHQueryParForm(DateTime.Now.AddDays(-1.0), DateTime.Now, withZone);
            f.ShowDialog();
            return new Tuple<bool, bool, bool, bool, DateTime, DateTime>(
                f.DialogResult == System.Windows.Forms.DialogResult.OK,
                f.WithUG, f.WithOPD, f.WithWinder,
                f.TimeFrom, f.TimeTo);
        }
        public static Tuple<bool, bool, DateTime, DateTime> GetWRRQueryParameters()
        {
            WRRQueryParForm f = new WRRQueryParForm(DateTime.Now.AddDays(-1.0), DateTime.Now);
            f.ShowDialog();
            return new Tuple<bool, bool, DateTime, DateTime>(
                f.DialogResult == System.Windows.Forms.DialogResult.OK,
                f.Cycles,
                f.TimeFrom, f.TimeTo);
        }
        public static void About(string version)
        {
            AboutForm f = new AboutForm(version);
            f.ShowDialog();
        }
    }
}
