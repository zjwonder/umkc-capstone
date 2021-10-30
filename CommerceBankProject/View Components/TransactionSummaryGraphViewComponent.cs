using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CommerceBankProject.Models;
using CommerceBankProject.Data;

namespace CommerceBankProject.View_Components
{
    public class TransactionSummaryGraphViewComponent : ViewComponent
    {

        private readonly CommerceBankDbContext _context;

        public TransactionSummaryGraphViewComponent(CommerceBankDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var model = _context.Users.find;
            //return await Task.FromResult((IViewComponentResult)View("SocialLinks", model));
            //var userID = "777777777";

            //var claim = User.FindFirst(ClaimTypes.NameIdentifier);

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
	                        AND trans.customerID = '777777777'
                        GROUP BY
	                        CONVERT(VARCHAR(7), trans.onDate, 126)
	                        ,trans.customerID
	                        ,trans.actID
	                        ,trans.actType";
            List<YearMonthAggregated_Transaction> tList = await _context.YearMonthAggregated_Transaction.FromSqlRaw(tQuery).ToListAsync();
            TAggregatedIndexViewModel vmod = new TAggregatedIndexViewModel(tList);

            return View(vmod);
        }
    }
}
