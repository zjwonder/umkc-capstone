using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class TCategoryAggregatedIndexViewModel
    {
        public List<YearMonthAggregated_CategoryTransactions> tList { get; set; }
        public List<AccountRecord> actList { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string descSearch { get; set; }
        public string debug { get; set; }

        public TCategoryAggregatedIndexViewModel(List<YearMonthAggregated_CategoryTransactions> aggregated_Transactions, List<AccountRecord> accounts, DateTime start, DateTime end, string desc = "")
        {
            tList = aggregated_Transactions;
            actList = accounts;
            descSearch = desc;
            fromDate = start;
            toDate = end;
        }
    }
}
