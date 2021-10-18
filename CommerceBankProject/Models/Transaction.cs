using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommerceBankProject.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        [Column(TypeName = "nvarchar(9)")]
        public string customerID { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string actID { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string actType { get; set; }
        [DataType(DataType.Date)]
        public DateTime onDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal balance { get; set; }
        [Column(TypeName = "nvarchar(2)")]
        public string transType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal amount { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string description { get; set; }
        public bool userEntered { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string category { get; set; }
    }
}
