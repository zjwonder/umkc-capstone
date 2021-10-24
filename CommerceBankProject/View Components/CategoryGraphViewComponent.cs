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
    public class CategoryGraphViewComponent : ViewComponent
    {
        private readonly CommerceBankDbContext _context;

        public CategoryGraphViewComponent(CommerceBankDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IViewComponentResult> InvokeAsync()
        {

            /*
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
            */
            string tQuery = @"
                        SELECT
                            CAST( DENSE_RANK() OVER (ORDER BY DATEADD(MONTH, DATEDIFF(MONTH, 0, trans_cat.onDate),0)
	                            , trans_cat.customerID, trans_cat.actID, trans_cat.actType, trans_cat.Category) AS INT) [ID]
                            ,trans_cat.customerID
                            ,trans_cat.actID
                            ,trans_cat.actType
                            ,trans_cat.Category
                            ,DATEADD(
                                MONTH
                                , DATEDIFF(MONTH, 0, trans_cat.onDate)
                                , 0) [MonthYearDate]
                            ,ABS(SUM(
	                            CASE
		                            WHEN trans_cat.transType = 'CR' THEN trans_cat.amount
		                            WHEN trans_cat.transType = 'DR' THEN (trans_cat.amount * -1)
		                            ELSE NULL END
                            )) [NetAmount]
                            FROM
                            (
	                            SELECT
		                            trans.*
	                            FROM
		                            [CommerceBankProject].[dbo].[Transaction] trans
	                            WHERE 1=1
		                            AND YEAR(trans.onDate) = 2019
		                            AND MONTH(trans.onDate) = 12
                            ) trans_cat
                            WHERE
	                            trans_cat.Category <> 'Income'
                            GROUP BY
                                trans_cat.customerID
                                ,trans_cat.actID
                                ,trans_cat.actType
                                ,trans_cat.Category
                                ,DATEADD(
                                    MONTH
                                    , DATEDIFF(MONTH, 0, trans_cat.onDate)
                                    , 0)
                            ORDER BY
	                            DATEADD(
		                            MONTH
		                            , DATEDIFF(MONTH, 0, trans_cat.onDate)
		                            , 0) DESC
	                            ,SUM(
		                            CASE
			                            WHEN trans_cat.transType = 'CR' THEN trans_cat.amount
			                            WHEN trans_cat.transType = 'DR' THEN (trans_cat.amount * -1)
			                            ELSE NULL END
	                            ) DESC";
            List<YearMonthAggregated_CategoryTransactions> tList = await _context.YearMonthAggregated_CategoryTransactions.FromSqlRaw(tQuery, "777777777").ToListAsync();
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
            record = await _context.Date.FromSqlRaw(dateQuery + " DESC", "777777777").FirstOrDefaultAsync();
            DateTime toDate = record.onDate;
            TCategoryAggregatedIndexViewModel vmod = new TCategoryAggregatedIndexViewModel(tList, actList, fromDate, toDate);

            return View(vmod);
        }
    }
}
