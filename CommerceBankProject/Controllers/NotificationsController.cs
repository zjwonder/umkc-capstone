using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CommerceBankProject.Data;
using CommerceBankProject.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CommerceBankProject.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly CommerceBankDbContext _context;

        public NotificationsController(CommerceBankDbContext context)
        {
            _context = context;
        }

        // GET: Notifications
        [Authorize]
        public async Task<IActionResult> Index()
        {
            

            //return View(vmod);
            return View(await _context.Notification.OrderByDescending(n => n.onDate).ToListAsync());
        }


        public async Task<IActionResult> Generate()
        {
            decimal amount = 3000;
            string customerID = "999999999";
            string query = "";
            query += "select temp.tyear, temp.tmonth, temp.amount from( ";
            query += "select year([Transaction].onDate) as tyear , month([Transaction].onDate) as tmonth, sum([Transaction].amount) as amount ";
            query += "from [Transaction] ";
            query += "where [Transaction].transType = 'DR' and customerID = {0} ";
            query += "group by year([Transaction].onDate), month([Transaction].onDate) ";
            query += ") as temp ";
            query += "where amount > {1} ";
            query += "order by temp.tyear DESC, temp.tmonth DESC;";
            List<MonthlyResult> monthlyList = await _context.MonthlyResult.FromSqlRaw(query, customerID, amount).ToListAsync();

           
            foreach (var month in monthlyList) {
                
                DateTime notificationDate = new DateTime(month.tyear,month.tmonth,1);
                notificationDate = notificationDate.AddMonths(1);
                string desc = "You have exceeded your monthly budget... :)";
                Notification notification = new Notification { customerID = "999999999", type = "Monthly Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();
            }



            decimal balance = 4000;
            

            query = @"select *
                    from[Transaction]
                    where transType = 'DR' and balance < {0} and customerID = {1}
                    order by onDate; ";

            var balanceList = await _context.Transaction.FromSqlRaw(query, balance,customerID).ToListAsync();


            foreach (var trans in balanceList)
            {

                string desc = "Your account is low ... :)";
                Notification notification = new Notification { customerID = customerID, type = "Low balance", description = desc, onDate = trans.onDate, read = false, saved = false };
                await _context.AddAsync(notification);
                await _context.SaveChangesAsync();
            }





            // return View("Index", await _context.Notification.ToListAsync());
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Settings()
        {
            return View(new NotificationSettings
            {
                customerID = "999999999",
                monthlyBudgetRule = 50,
                monthlyBudgetRuleActive = false,
                balanceRule = 50,
                balanceRuleActive = true, 
                choresRule = 50,
                choresRuleActive = false,
                clothingRule = 50,
                clothingRuleActive = false,
                eatingOutRule = 50,
                eatingOutRuleActive = false,
                essentialsRule = 50,
                essentialsRuleActive = false,
                foodRule = 50,
                foodRuleActive = false,
                funRule = 50,
                funRuleActive = false,
                gasRule = 50,
                gasRuleActive = false,
                otherRule = 50,
                otherRuleActive = false,
                phoneRule = 50,
                phoneRuleActive = false
            });
        }

        public async Task<IActionResult> SettingsChange(bool monthlyBudgetActive, decimal monthlyBudget,bool balanceActive, decimal balance)
        {
            return RedirectToAction(nameof(Settings));
        }


        public async Task<IActionResult> DeleteAll()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
            string query = "delete from Notification where customerID = {0};";
            await _context.Database.ExecuteSqlRawAsync(query, user.customerID);


            return RedirectToAction(nameof(Index));
            
        }

        // GET: Notifications/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification
                .FirstOrDefaultAsync(m => m.ID == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,customerID,type,description,onDate,read,saved")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,customerID,type,description,onDate,read,saved")] Notification notification)
        {
            if (id != notification.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification
                .FirstOrDefaultAsync(m => m.ID == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification = await _context.Notification.FindAsync(id);
            _context.Notification.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool NotificationExists(int id)
        {
            return _context.Notification.Any(e => e.ID == id);
        }
    }
}
