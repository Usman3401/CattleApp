using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class StatusAnimal
    {
        [Key]
        public int StatusHistoryID { get; set; }
        public int AnimalID { get; set; } 
        public int StatusID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Notes { get; set; }
    }
}
