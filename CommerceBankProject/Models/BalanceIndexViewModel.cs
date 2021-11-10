using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class BalanceIndexViewModel
    {
        public List<Balance> tList { get; set; }
        public BalanceIndexViewModel(List<Balance> balances)
        {
            tList = balances;
        }
    }
}
