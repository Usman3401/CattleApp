using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class Heifer
    {
        public int HeiferHistoryID { get; set; }
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Weight { get; set; }
        public string Notes { get; set; }
    }
}
