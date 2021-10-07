using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommerceBankProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CommerceBankProject.Models;

namespace CommerceBankProject.Data
{
    public class CommerceBankDbContext : IdentityDbContext<ApplicationUser>
    {
        public CommerceBankDbContext(DbContextOptions<CommerceBankDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AccountRecord>().HasNoKey();
            builder.Entity<DateRecord>().HasNoKey();
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<AccountRecord> Account { get; set; }
        public DbSet<CustomerRecord> Customer { get; set; }
        public DbSet<DateRecord> Date { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<YearMonthAggregated_Transaction> YearMonthAggregated_Transaction { get; set; }
        public DbSet<YearMonthAggregated_CategoryTransactions> YearMonthAggregated_CategoryTransactions { get; set; }
    }
}
