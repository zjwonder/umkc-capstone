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
    public class TransactionSummaryGraphViewComponent : ViewComponent
    {

        private readonly CommerceBankDbContext _context;

        public TransactionSummaryGraphViewComponent(CommerceBankDbContext context)
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
	                        CAST( DENSE_RANK() OVER (ORDER BY CONVERT(VARCHAR(7), trans.onDate, 126)
		                        , trans.customerID, trans.actID) AS INT) [ID]
	                        ,CONVERT(VARCHAR(7), trans.onDate, 126) [MonthName]
	                        ,trans.customerID
	                        ,trans.actID
	                        ,trans.actType
	                        ,COUNT(trans.ID) [NumTransactions]
	                        ,SUM(
		                        CASE

			                        WHEN trans.transType = 'CR' THEN trans.amount

			                        WHEN trans.transType = 'DR' THEN (trans.amount * -1)

			                        ELSE NULL END
	                        ) [NetAmount]
                        FROM[CommerceBankProject].[dbo].[Transaction] trans
                        WHERE 1 = 1
	                        AND trans.customerID = {0}
                            AND trans.actType = 'Checking'
                        GROUP BY
	                        CONVERT(VARCHAR(7), trans.onDate, 126)
	                        ,trans.customerID
	                        ,trans.actID
	                        ,trans.actType";
            List<YearMonthAggregated_Transaction> tList = await _context.YearMonthAggregated_Transaction.FromSqlRaw(tQuery, user.customerID).ToListAsync();
            TAggregatedIndexViewModel vmod = new TAggregatedIndexViewModel(tList);

            return View(vmod);
        }
    }
}
