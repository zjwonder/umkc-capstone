using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class CustomerRecord
    {
        [Key]
        public string customerID { get; set; }
        public string email { get; set; }
        public bool claimed { get; set; }
    }
}
