using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BholaCattleApp.Models
{
    public class Transaction
    {
        public int TransactionHistoryID { get; set; }
        public DateTime TransDate { get; set; }
        public string Type { get; set; }
        public int ItemID { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
    }
}
