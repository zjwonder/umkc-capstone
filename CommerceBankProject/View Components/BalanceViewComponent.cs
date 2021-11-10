using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CommerceBankProject.Models;
using CommerceBankProject.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace CommerceBankProject.View_Components
{
    public class BalanceViewComponent : ViewComponent
    {
        private readonly CommerceBankDbContext _context;

        public BalanceViewComponent(CommerceBankDbContext context)
        {
            _context = context;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
            string tQuery = @"SELECT
	                            CAST( DENSE_RANK() OVER (ORDER BY 
		                            trans_most_recent.actType
		                            , trans_most_recent.balance) AS INT) [ID]
	                            --,trans_most_recent.customerID
	                            ,trans_most_recent.actType
	                            ,trans_most_recent.balance [Balance]
                            FROM
                            (
	                            SELECT --TOP 2
		                            *
		                            ,ROW_NUMBER() OVER(PARTITION BY trans.customerID, trans.actID ORDER BY trans.onDate DESC) RN
		                            --trans.balance
	                            FROM
		                            [CommerceBankProject].[dbo].[Transaction] trans
	                            WHERE 1=1
		                            AND trans.customerID = {0}
                            ) trans_most_recent
                            WHERE 1=1
	                            AND trans_most_recent.RN = 1";
            List<Balance> tList = await _context.Balance.FromSqlRaw(tQuery, user.customerID).ToListAsync();
            BalanceIndexViewModel vmod = new BalanceIndexViewModel(tList);

            return View(vmod);
        }
    }
}
