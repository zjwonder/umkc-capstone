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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userID);

            IQueryable<Transaction> transactionIQ = from t in _context.Transaction.Where(
                t => t.customerID == user.customerID) select t;

            List<Transaction> transactionList = await transactionIQ.AsNoTracking().ToListAsync();
            //List<AccountRecord> actList = PopulateActList(transactionList);

            List<AccountRecord> actList = transactionList
                .GroupBy(p => p.actID)
                .Select(g => g.First())
                .Select(x => new AccountRecord { actID = x.actID, actType = x.actType})
                .ToList();

            TIndexViewModel vmod = new TIndexViewModel(
                transactions: transactionList,
                start: transactionList.FirstOrDefault().onDate,
                end: transactionList.LastOrDefault().onDate,
                accounts: actList);

            return View(vmod);
        }

        // GET: Filtered Transactions
        [Authorize]
        public async Task<IActionResult> FilterIndex(string actFilter, string descFilter, string fromDate, string toDate, string pageNumber)
        {
            // Get user claim
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();

            // Parse date strings into date objects
            DateTime tDate = DateTime.Parse(toDate);
            DateTime fDate = DateTime.Parse(fromDate);

            IQueryable<Transaction> transactionIQ = from t in _context.Transaction.Where(
                t => t.customerID == user.customerID 
                && t.onDate >= fDate 
                && t.onDate < tDate) select t;
            //List<AccountRecord> actList = PopulateActList(await transactionIQ.AsNoTracking().ToListAsync());
            List<AccountRecord> actList = transactionIQ.AsNoTracking().ToList()
                .GroupBy(p => p.actID)
                .Select(g => g.First())
                .Select(x => new AccountRecord { actID = x.actID, actType = x.actType })
                .ToList();

            if (actFilter != "all")
            {
                transactionIQ = transactionIQ.Where(t => t.actID == actFilter);

            }
            if (!string.IsNullOrEmpty(descFilter))
            {
                transactionIQ = transactionIQ.Where(t => t.description == actFilter);
            }

            List<Transaction> tList = await transactionIQ.AsNoTracking().ToListAsync();

            TIndexViewModel vmod = new TIndexViewModel(
                accounts: actList, 
                transactions: tList, 
                start: fDate, 
                end: tDate, 
                desc: descFilter, 
                page: int.Parse(pageNumber), 
                acct: actFilter);

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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,customerID,actID,actType,onDate,balance,transType,amount,description,userEntered,category")] Transaction transaction)
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

        //private List<AccountRecord> PopulateActList(List<Transaction> transactionList)
        //{
        //    HashSet<AccountRecord> set = new HashSet<AccountRecord>();
        //    foreach (var f in transactionList)
        //    {
        //        AccountRecord temp = new AccountRecord();

        //        temp.actID = f.actID;
        //        temp.actType = f.actType;

        //        set.Add(temp);
        //    }
        //    List<AccountRecord> actList;
        //    return actList = set.ToList();
        //    //List<AccountRecord> accounts = await transactionIQ
        //    //    .GroupBy(t => t.actID)
        //    //    .Select(group => new AccountRecord
        //    //    {
        //    //        actID = group.Key, 
        //    //        actType = group.
        //    //    }).ToListAsync();

        //    // List<string> actIDList = await transactionIQ.Select(x => x.actID).Distinct().ToListAsync();
        //    // List<AccountRecord> actList2 = await transactionIQ
        //    //     .Select(x => new AccountRecord { actID = x.actID, actType = x.actType}).ToListAsync();

        //    // List<AccountRecord> actList = await transactionIQ
        //    //     .Select(x => new AccountRecord { actID = x.actID, actType = x.actType })
        //    //     .GroupBy(g => g.actID)
        //    //     .Select(a => a.First())
        //    //     .ToListAsync();
        //}
    }
}
