using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class Balance
    {
        public int ID { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string actType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal balance { get; set; }
    }
}
