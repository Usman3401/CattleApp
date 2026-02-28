using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class Milking
    {
        public int MilkingHistoryID { get; set; }
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public decimal MorningQty { get; set; }
        public decimal EveningQty { get; set; }
        public decimal NightQty { get; set; }
        public DateTime MilkingDate { get; set; }
    }
}
