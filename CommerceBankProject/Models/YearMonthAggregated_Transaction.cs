using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommerceBankProject.Models
{
    public class YearMonthAggregated_Transaction
    {
        public int ID { get; set; }
        [DataType(DataType.Date)]
        public DateTime MonthYearDate { get; set; }
        [Column(TypeName = "nvarchar(9)")]
        public string customerID { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string actID { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string actType { get; set; }
        public int NumTransactions { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }
    }
}
