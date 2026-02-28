using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class Medicine
    {
        public int MedicineHistoryID { get; set; }
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public DateTime MedicineDate { get; set; }
        public string Name { get; set; }
        public decimal Dosage { get; set; }
        public string Note { get; set; }
        
    }
}
