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
                                    CAST( DENSE_RANK() OVER (ORDER BY DATEADD(MONTH, DATEDIFF(MONTH, 0, trans.onDate),0)
		, trans.customerID, trans.actID) AS INT) [ID]
                                    ,DATEADD(
                                        MONTH
                                        , DATEDIFF(MONTH, 0, trans.onDate)
                                        , 0) [MonthYearDate]
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
                                GROUP BY
                                    DATEADD(MONTH, DATEDIFF(MONTH, 0, trans.onDate), 0)
	                                ,trans.customerID
	                                ,trans.actID
	                                ,trans.actType
                                ORDER BY
                                    DATEADD(MONTH, DATEDIFF(MONTH, 0, trans.onDate), 0)
	                                ,trans.customerID
	                                ,trans.actID
	                                ,trans.actType";
            List<YearMonthAggregated_Transaction> tList = await _context.YearMonthAggregated_Transaction.FromSqlRaw(tQuery, "777777777").ToListAsync();
            string actQuery = "Select distinct actID, actType from [Transaction] where customerID = {0}";
            List<AccountRecord> actList = await _context.Account.FromSqlRaw(actQuery, "777777777").ToListAsync();
            string dateQuery = @"SELECT 
	                                TOP 1 DATEADD(
		                                MONTH
		                                , DATEDIFF(MONTH, 0, trans.onDate)
		                                ,0) [onDate]
                                FROM [Transaction] trans
                                WHERE customerID = {0} 
                                ORDER BY ID";
            DateRecord record = await _context.Date.FromSqlRaw(dateQuery, "777777777").FirstOrDefaultAsync();
            DateTime fromDate = record.onDate;
            record = await  _context.Date.FromSqlRaw(dateQuery + " DESC", "777777777").FirstOrDefaultAsync();
            DateTime toDate = record.onDate;
            TAggregatedIndexViewModel vmod = new TAggregatedIndexViewModel(tList, actList, fromDate, toDate);

            return View(vmod);
        }
    }
}
