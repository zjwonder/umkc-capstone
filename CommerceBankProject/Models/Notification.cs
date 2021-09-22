using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class Notification
    {
        public int ID { get; set; }
        [Column(TypeName = "nvarchar(9)")]
        public string customerID { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string type { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string description { get; set; }
        public DateTime onDate { get; set; }
        public bool read { get; set; }
        public bool saved { get; set; }
    }
}
