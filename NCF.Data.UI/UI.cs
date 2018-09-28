using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCF.Data.UI {
    public class UI {
        public static object[] SelectFromList(IEnumerable list) {
            return Form1.SelectFromList(list);
        }
    }
}
