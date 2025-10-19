using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class Pregnant
    {
        [Key]
        public int PregnantHistoryID { get; set; }
        public int AnimalID { get; set; }
        public int StatusID { get; set; }
        public DateTime PregnantDate { get; set; }
        public DateTime DeliverDate { get; set; }
        public string Gender { get; set; }
        public string Result { get; set; }
        public string Notes { get; set; }
    }
}
