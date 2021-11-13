using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class MonthlyResult
    {
        //public int tId { get; set; }
        public int tyear { get; set; }
        public int tmonth { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal amount { get; set; }

    }
}
