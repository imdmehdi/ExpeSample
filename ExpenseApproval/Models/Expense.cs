using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseApproval.Models
{
   
    public class Expense
    {
        public int ExpenseId { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }

        public string UpdatedBy { get; set; }

        //public Status Status { get; set; }
        public string ExpenseRequesterID { get; set; }

        public ExpenseStatus Status { get; set; }

    }
    public enum ExpenseStatus
    {
        Submitted,
        Approved,
        Rejected
    }
   
}
