using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class UserTransactions
    {
        public Transaction transaction { get; set; }
        public List<AccountRecord> userAccounts { get; set; }

    }
}
