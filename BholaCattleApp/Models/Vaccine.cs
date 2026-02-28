using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class Vaccine
    {
        public int VaccineHistoryID { get; set; }
        public string Name { get; set; }
        public decimal Qty { get; set; }
        public int Price { get; set; }
    }
}
