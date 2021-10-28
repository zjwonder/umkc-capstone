using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class TIndexViewModel
    {
        public List<Transaction> tList { get; set; }
        public List<AccountRecord> actList { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string descSearch { get; set; }
        public string debug { get; set; }
        public int pageNumber { get; set; }
        public string account { get; set;  }

        public int pageSize { get; set; } = 10;

        public TIndexViewModel(List<Transaction> transactions, DateTime start, DateTime end, string desc="", int PageNumber = 1, string acct = "all")
        {
            tList = transactions;
            //actList = transactions.ForEach();
            descSearch = desc;
            fromDate = start;
            toDate = end;
            pageNumber = PageNumber;
            account = acct;

        }
    }
}
