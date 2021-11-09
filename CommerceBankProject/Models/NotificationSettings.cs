using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class NotificationSettings
    {
        
        [Key]   
        public string customerID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal monthlyBudgetRule { get; set; }
        public  bool monthlyBudgetRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal balanceRule { get; set; }
        public bool balanceRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal choresRule { get; set; }
        public bool choresRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal clothingRule { get; set; }
        public bool clothingRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal eatingOutRule { get; set; }
        public bool eatingOutRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal essentialsRule { get; set; }
        public bool essentialsRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal foodRule { get; set; }
        public bool foodRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal funRule { get; set; }
        public bool funRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal gasRule { get; set; }
        public bool gasRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal phoneRule { get; set; }
        public bool phoneRuleActive { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal otherRule { get; set; }
        public bool otherRuleActive { get; set; }
        [Column(TypeName = "time(7)")]
        public TimeSpan startTimeRule { get; set; }
        [Column(TypeName = "time(7)")]
        public TimeSpan endTimeRule { get; set; }
        public bool timeRuleActive { get; set; }


    }
}
