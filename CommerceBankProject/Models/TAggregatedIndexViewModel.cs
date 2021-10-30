using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class TAggregatedIndexViewModel
    {
        public List<YearMonthAggregated_Transaction> tList { get; set; }

        public TAggregatedIndexViewModel(List<YearMonthAggregated_Transaction> aggregated_Transactions)
        {
            tList = aggregated_Transactions;
        }
    }
}
