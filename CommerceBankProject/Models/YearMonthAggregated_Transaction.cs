using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CommerceBankProject.Data;
using CommerceBankProject.Models;
using System.Security.Claims;

namespace CommerceBankProject.Models
{
    public class YearMonthAggregated_Transaction
    {

        public int ID { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string MonthName { get; set; }
        [Column(TypeName = "nvarchar(30)")]
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
