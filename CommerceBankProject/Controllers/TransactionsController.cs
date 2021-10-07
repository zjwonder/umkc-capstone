using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CommerceBankProject.Data;
using CommerceBankProject.Models;
using System.Security.Claims;

namespace CommerceBankProject.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly CommerceBankDbContext _context;

        public TransactionsController(CommerceBankDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
            string tQuery = "Select * from [Transaction] where customerID = {0} order by onDate desc;";
            List<Transaction> tList = await _context.Transaction.FromSqlRaw(tQuery, user.customerID).ToListAsync();
            string actQuery = "Select distinct actID, actType from [Transaction] where customerID = {0}";
            List<AccountRecord> actList = await _context.Account.FromSqlRaw(actQuery, user.customerID).ToListAsync();
            string dateQuery = "Select top 1 onDate from [Transaction] where customerID = {0} order by ID";
            DateRecord record = await _context.Date.FromSqlRaw(dateQuery, user.customerID).FirstOrDefaultAsync();
            DateTime fromDate = record.onDate;
            record = await _context.Date.FromSqlRaw(dateQuery+" desc", user.customerID).FirstOrDefaultAsync();
            DateTime toDate = record.onDate;
            TIndexViewModel vmod = new TIndexViewModel(tList, actList, fromDate, toDate);
            
            return View(vmod);
        }

        // GET: Graphs
        [Authorize]
        public async Task<IActionResult> Graphs()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
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
            List<YearMonthAggregated_Transaction> tList = await _context.YearMonthAggregated_Transaction.FromSqlRaw(tQuery, user.customerID).ToListAsync();
            string actQuery = "Select distinct actID, actType from [Transaction] where customerID = {0}";
            List<AccountRecord> actList = await _context.Account.FromSqlRaw(actQuery, user.customerID).ToListAsync();
            string dateQuery = @"SELECT 
	                                TOP 1 DATEADD(
		                                MONTH
		                                , DATEDIFF(MONTH, 0, trans.onDate)
		                                ,0) [onDate]
                                FROM [Transaction] trans
                                WHERE customerID = {0} 
                                ORDER BY ID";
            DateRecord record = await _context.Date.FromSqlRaw(dateQuery, user.customerID).FirstOrDefaultAsync();
            DateTime fromDate = record.onDate;
            record = await _context.Date.FromSqlRaw(dateQuery + " DESC", user.customerID).FirstOrDefaultAsync();
            DateTime toDate = record.onDate;
            TAggregatedIndexViewModel vmod = new TAggregatedIndexViewModel(tList, actList, fromDate, toDate);

            return View(vmod);
        }

        // GET: Graphs
        [Authorize]
        public async Task<IActionResult> Donut()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
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
		                        , CASE
			                        WHEN
				                        trans.[description] IN 
					                        ('ATM'
					                        ,'Car wash'
					                        ,'CVS'
					                        ,'Dollar General'
					                        ,'Ford Service'
					                        ,'Home Depot'
					                        ,'Jiffy Lube'
					                        ,'KC Police - Speeding Ticket'
					                        ,'Nebraska Furniture Mart'
					                        ,'Petsmart'
					                        ,'Starbucks gift card')
			                        THEN
				                        'Chores'
			                        WHEN 
				                        trans.[description] IN ('Maurices')
			                        THEN
				                        'Clothing'
			                        WHEN 
				                        trans.[description] IN 
					                        ('Bravos'
					                        ,'Bristol'
					                        ,'Buffalo Wild Wings'
					                        ,'Burger King'
					                        ,'Cheesecake Factory'
					                        ,'Chipotle'
					                        ,'Denny''s'
					                        ,'Hoolihans'
					                        ,'Jose Peppers'
					                        ,'KFC'
					                        ,'Lucky''s'
					                        ,'Manny''s'
					                        ,'McFaddens'
					                        ,'MOD Pizza'
					                        ,'O''Charley''s'
					                        ,'Olive Garden'
					                        ,'O''Reilly''s'
					                        ,'Panda Express'
					                        ,'Panera Bread'
					                        ,'Pizza Hut'
					                        ,'Pizza Ranch'
					                        ,'Red Lobster'
					                        ,'Red Robin'
					                        ,'Scooters'
					                        ,'Starbucks'
					                        ,'Taco Bell'
					                        ,'Tanners')
			                        THEN
				                        'Eating Out'
			                        WHEN 
				                        trans.[description] IN 
					                        ('Airline Ticket'
					                        ,'AT&T'
					                        ,'ATM'
					                        ,'Bank of America Credit Card Payment'
					                        ,'BoA Credit Card Payment'
					                        ,'Check to Brother'
					                        ,'Check to Sister'
					                        ,'Commerce Bank Credit Card Payment'
					                        ,'Doctor'
					                        ,'Doctor Visit'
					                        ,'Google Fiber'
					                        ,'KCPL'
					                        ,'Nationwide'
					                        ,'Nebraska Furniture Mart'
					                        ,'Rent'
					                        ,'State Farm'
					                        ,'Student Loans'
					                        ,'Target'
					                        ,'Taxes'
					                        ,'Walmart')
			                        THEN
				                        'Essentials'
			                        WHEN 
				                        trans.[description] IN 
					                        ('Hyvee'
					                        ,'Price Chopper')
			                        THEN
				                        'Food'
			                        WHEN 
				                        trans.[description] IN 
					                        ('7 Eleven'
					                        ,'Amazon'
					                        ,'Best Buy'
					                        ,'Bowling'
					                        ,'Brew Top'
					                        ,'Bristol'
					                        ,'CVS'
					                        ,'Dave and Busters'
					                        ,'Famous Footwear'
					                        ,'Hallmark'
					                        ,'Hobby Lobby'
					                        ,'Joy Wok'
					                        ,'Laser Rock'
					                        ,'Neo''s'
					                        ,'Netflix'
					                        ,'Pottery Barn'
					                        ,'Red box'
					                        ,'Redbox'
					                        ,'Sears'
					                        ,'The Loft'
					                        ,'Toys R Us'
					                        ,'Uber')
			                        THEN
				                        'Fun'
			                        WHEN 
				                        trans.[description] IN ('QuikTrip')
			                        THEN
				                        'Gas'
			                        WHEN 
				                        trans.[description] IN 
					                        ('Cash Deposit'
					                        ,'Check Deposit'
					                        ,'Check from friend'
					                        ,'Check from grandma'
					                        ,'Check from mom'
					                        ,'Christmas check from Grandma'
					                        ,'Interest'
					                        ,'Payday'
					                        ,'Payroll')
			                        THEN
				                        'Income'
			                        WHEN 
				                        trans.[description] IN ('Verizon')
			                        THEN
				                        'Phone'
			                        WHEN 
				                        trans.[description] LIKE 'Transfer%'
			                        THEN
				                        'Income'
			                        ELSE 'Other' END [Category]
	                        FROM
		                        [CommerceBankProject].[dbo].[Transaction] trans
	                        WHERE 1=1
		                        AND YEAR(trans.onDate) = 2019
		                        AND MONTH(trans.onDate) = 12
                        ) trans_cat
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
            List<YearMonthAggregated_CategoryTransactions> tList = await _context.YearMonthAggregated_CategoryTransactions.FromSqlRaw(tQuery, user.customerID).ToListAsync();
            string actQuery = "Select distinct actID, actType from [Transaction] where customerID = {0}";
            List<AccountRecord> actList = await _context.Account.FromSqlRaw(actQuery, user.customerID).ToListAsync();
            string dateQuery = @"SELECT 
	                                TOP 1 DATEADD(
		                                MONTH
		                                , DATEDIFF(MONTH, 0, trans.onDate)
		                                ,0) [onDate]
                                FROM [Transaction] trans
                                WHERE customerID = {0} 
                                ORDER BY ID";
            DateRecord record = await _context.Date.FromSqlRaw(dateQuery, user.customerID).FirstOrDefaultAsync();
            DateTime fromDate = record.onDate;
            record = await _context.Date.FromSqlRaw(dateQuery + " DESC", user.customerID).FirstOrDefaultAsync();
            DateTime toDate = record.onDate;
            TCategoryAggregatedIndexViewModel vmod = new TCategoryAggregatedIndexViewModel(tList, actList, fromDate, toDate);

            return View(vmod);
        }

        [Authorize]
        public async Task<IActionResult> FilterIndex(string actFilter, string descFilter, string fromDate, string toDate)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
            string[] splitDate = toDate.Split('-');
            DateTime tDate = new DateTime(int.Parse(splitDate[0]), int.Parse(splitDate[1]), int.Parse(splitDate[2]));
            toDate = string.Format("{0:yyyy-MM-dd}", tDate.AddDays(1));
            string tQuery = "Select * from [Transaction] where customerID = {0}";
            tQuery += " and onDate >= {1} and onDate < {2}";
            if (!string.IsNullOrEmpty(descFilter))
            {
                tQuery += " and description like '%' + {3} + '%'";
            }
            List<Transaction> tList;
            if (actFilter == "all")
            {
                tQuery += " order by onDate desc";
                tList = await _context.Transaction.FromSqlRaw(tQuery, user.customerID, fromDate, toDate, descFilter).ToListAsync();
            }
            else
            {
                tQuery += " and actID = {4} order by onDate desc";
                tList = await _context.Transaction.FromSqlRaw(tQuery, user.customerID, fromDate, toDate, descFilter, actFilter).ToListAsync();
            }
            string actQuery = "Select distinct actID, actType from [Transaction] where customerID = {0}";
            splitDate = fromDate.Split('-');
            DateTime fDate = new DateTime(int.Parse(splitDate[0]), int.Parse(splitDate[1]), int.Parse(splitDate[2]));
            List<AccountRecord> actList = await _context.Account.FromSqlRaw(actQuery, user.customerID).ToListAsync();
            TIndexViewModel vmod = new TIndexViewModel(tList, actList, fDate, tDate, descFilter);
            
            return View("Index", vmod);
        }

        // GET: Transactions/Details/
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,customerID,actID,actType,onDate,balance,transType,amount,description,userEntered")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.ID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.ID == id);
        }
    }
}
