using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class Feeding
    {
        public int FeedingHistoryID { get; set; }
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public string Type { get; set; }
        public decimal Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalCount { get; set; }
    }
}
