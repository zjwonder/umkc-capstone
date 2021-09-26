using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CommerceBankProject.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(9)")]
        public string customerID { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string firstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string lastName { get; set; }
        public bool darkMode { get; set; }
    }
}
