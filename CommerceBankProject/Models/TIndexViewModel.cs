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

        public TIndexViewModel(List<Transaction> transactions, List<AccountRecord> accounts, DateTime start, DateTime end, string desc="")
        {
            tList = transactions;
            actList = accounts;
            descSearch = desc;
            fromDate = start;
            toDate = end;
        }
    }
}
