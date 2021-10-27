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
            return View(await _context.Notification.OrderByDescending(n => n.onDate).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Generate()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
            string query = "";
            query = @"Select *
            from [NotificationSettings]
            where customerID = {0}";
            var settings = await _context.NotificationSettings.FromSqlRaw(query, user.customerID).FirstOrDefaultAsync();

            query = @"select temp.tyear, temp.tmonth, temp.amount from( 
            select year([Transaction].onDate) as tyear , month([Transaction].onDate) as tmonth, sum([Transaction].amount) as amount
            from [Transaction]
            where [Transaction].category = {0} and customerID = {1}
            group by ([Transaction].category), year([Transaction].onDate), month([Transaction].onDate)
            ) as temp 
            where amount > {2}
            order by temp.tyear DESC, temp.tmonth DESC;";

            if(settings.monthlyBudgetRuleActive)
            {
                string queryMonthly = @"select temp.tyear, temp.tmonth, temp.amount from( 
                select year([Transaction].onDate) as tyear , month([Transaction].onDate) as tmonth, sum([Transaction].amount) as amount
                from [Transaction]
                where [Transaction].transType = 'DR' and customerID = {0}
                group by year([Transaction].onDate), month([Transaction].onDate)
                ) as temp 
                where amount > {1}
                order by temp.tyear DESC, temp.tmonth DESC;";

                List<MonthlyResult> monthlyList = await _context.MonthlyResult.FromSqlRaw(queryMonthly, user.customerID, settings.monthlyBudgetRule).ToListAsync();

                foreach (var month in monthlyList)
                {
                    DateTime notificationDate = new DateTime(month.tyear, month.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly budget of $" + settings.monthlyBudgetRule.ToString();
                    Notification notification = new Notification { customerID = user.customerID, type = "Monthly Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if(settings.balanceRuleActive)
            {
                string queryTrans = @"select *
                from[Transaction]
                where transType = 'DR' and balance < {0} and customerID = {1}
                order by onDate; ";

                var balanceList = await _context.Transaction.FromSqlRaw(queryTrans, settings.balanceRule, user.customerID).ToListAsync();

                foreach (var trans in balanceList)
                {
                    string desc = "Your account is below your notification balance of $" + settings.balanceRule.ToString();
                    Notification notification = new Notification { customerID = user.customerID, type = "Low balance", description = desc, onDate = trans.onDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.choresRuleActive)
            {
                var choresList = await _context.MonthlyResult.FromSqlRaw(query, "chores", user.customerID, settings.choresRule).ToListAsync();

                foreach (var chores in choresList)
                {
                    DateTime notificationDate = new DateTime(chores.tyear, chores.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly chores budget of $" + settings.choresRule.ToString();
                    Notification notification = new Notification { customerID = user.customerID, type = "Chores Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.clothingRuleActive)
            {
                var clothingList = await _context.MonthlyResult.FromSqlRaw(query, "clothing", user.customerID, settings.clothingRule).ToListAsync();

                foreach (var clothing in clothingList)
                {
                    DateTime notificationDate = new DateTime(clothing.tyear, clothing.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly clothing budget of $" + settings.clothingRule.ToString();
                    Notification notification = new Notification { customerID = user.customerID, type = "Clothing Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.eatingOutRuleActive)
            {
                var eatingOutList = await _context.MonthlyResult.FromSqlRaw(query, "eatingOut", user.customerID, settings.eatingOutRule).ToListAsync();

                foreach (var eatingOut in eatingOutList)
                {
                    DateTime notificationDate = new DateTime(eatingOut.tyear, eatingOut.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly eating out budget of $" + settings.eatingOutRule.ToString();
                    Notification notification = new Notification { customerID = user.customerID, type = "Eating Out Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.essentialsRuleActive)
            {
                var essentialsList = await _context.MonthlyResult.FromSqlRaw(query, "essentials", user.customerID, settings.essentialsRule).ToListAsync();

                foreach (var essentials in essentialsList)
                {
                    DateTime notificationDate = new DateTime(essentials.tyear, essentials.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly essentials budget of $" + settings.essentialsRule.ToString();
                    Notification notification = new Notification { customerID = user.customerID, type = "Essentials Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.foodRuleActive)
            {
                var foodList = await _context.MonthlyResult.FromSqlRaw(query, "food", user.customerID, settings.foodRule).ToListAsync();

                foreach (var food in foodList)
                {
                    DateTime notificationDate = new DateTime(food.tyear, food.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly food budget of $" + settings.foodRule.ToString(); ;
                    Notification notification = new Notification { customerID = user.customerID, type = "Food Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.funRuleActive)
            {
                var funList = await _context.MonthlyResult.FromSqlRaw(query, "fun", user.customerID, settings.funRule).ToListAsync();

                foreach (var fun in funList)
                {
                    DateTime notificationDate = new DateTime(fun.tyear, fun.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly fun budget of $" + settings.funRule.ToString(); ;
                    Notification notification = new Notification { customerID = user.customerID, type = "Fun Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.gasRuleActive)
            {
                var gasList = await _context.MonthlyResult.FromSqlRaw(query, "gas", user.customerID, settings.gasRule).ToListAsync();

                foreach (var fun in gasList)
                {
                    DateTime notificationDate = new DateTime(fun.tyear, fun.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly gas budget of $" + settings.gasRule.ToString(); ;
                    Notification notification = new Notification { customerID = user.customerID, type = "Gas Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.phoneRuleActive)
            {
                var phoneList = await _context.MonthlyResult.FromSqlRaw(query, "phone", user.customerID, settings.phoneRule).ToListAsync();

                foreach (var phone in phoneList)
                {
                    DateTime notificationDate = new DateTime(phone.tyear, phone.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly phone budget of $" + settings.phoneRule.ToString(); ;
                    Notification notification = new Notification { customerID = user.customerID, type = "Phone Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.otherRuleActive)
            {
                var otherList = await _context.MonthlyResult.FromSqlRaw(query, "other", user.customerID, settings.otherRule).ToListAsync();

                foreach (var other in otherList)
                {
                    DateTime notificationDate = new DateTime(other.tyear, other.tmonth, 1);
                    notificationDate = notificationDate.AddMonths(1);
                    string desc = "You have exceeded your monthly other budget of $" + settings.otherRule.ToString(); ;
                    Notification notification = new Notification { customerID = user.customerID, type = "Other Budget", description = desc, onDate = notificationDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }
            if (settings.timeRuleActive)
            {
                int result = TimeSpan.Compare(settings.startTimeRule, settings.endTimeRule);
                if (result == 1)
                {
                    query = @"select *
                    from[Transaction]
                    where (CAST(onDate as time) >= {0} or CAST(onDate as time) < {1})
                    and customerID = {2} and [Transaction].transType = 'DR';";
                }
                else
                {
                    query = @"select *
                    from[Transaction]
                    where (CAST(onDate as time) >= {0} and CAST(onDate as time) < {1})
                    and customerID = {2} and [Transaction].transType = 'DR';";
                }

                var timeList = await _context.Transaction.FromSqlRaw(query, settings.startTimeRule, settings.endTimeRule, user.customerID).ToListAsync();

                string displayStartTime = new DateTime().Add(settings.startTimeRule).ToString("hh:mm tt");
                string displayEndTime = new DateTime().Add(settings.endTimeRule).ToString("hh:mm tt");

                foreach (var time in timeList)
                {
                    string desc = "You have a transaction out of your time frame of " + displayStartTime + " to " + displayEndTime;
                    Notification notification = new Notification { customerID = user.customerID, type = "Time frame", description = desc, onDate = time.onDate, read = false, saved = false };
                    await _context.AddAsync(notification);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Settings()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
            var settings = await _context.NotificationSettings.Where(s => s.customerID == user.customerID).FirstOrDefaultAsync();
            return View(settings);
        }

        [Authorize]
        public async Task<IActionResult> SettingsChange(bool monthlyBudgetActive, decimal monthlyBudget, bool balanceActive, decimal balance,
            bool choresRuleActive, decimal chores, bool clothingRuleActive, decimal clothing, bool eatingOutRuleActive, decimal eatingOut,
            bool essentialsRuleActive, decimal essentials, bool foodRuleActive, decimal food, bool funRuleActive, decimal fun, bool gasRuleActive,
            decimal gas, bool phoneRuleActive, decimal phone, bool otherRuleActive, decimal other, bool timeRuleActive, string startTimeRule,
            string endTimeRule)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            string userID = claim.Value;
            var user = await _context.Users.Where(u => u.Id == userID).FirstOrDefaultAsync();
            string query = "";

            query += @"Update NotificationSettings 
            Set monthlyBudgetRule = {0}, monthlyBudgetRuleActive = {1}, balanceRule = {2}, balanceRuleActive = {3}, 
            choresRule = {4}, choresRuleActive = {5}, clothingRule = {6}, clothingRuleActive = {7}, eatingOutRule = {8}, 
            eatingOutRuleActive = {9}, essentialsRule = {10}, essentialsRuleActive = {11}, foodRule = {12}, foodRuleActive = {13},
            funRule = {14}, funRuleActive = {15}, gasRule = {16}, gasRuleActive = {17}, phoneRule = {18}, phoneRuleActive = {19},
            otherRule = {20}, otherRuleActive = {21}, startTimeRule = {22}, endTimeRule = {23}, timeRuleActive = {24}
            Where customerID = {25}";

            await _context.Database.ExecuteSqlRawAsync(query, monthlyBudget, monthlyBudgetActive, balance, balanceActive, chores,
                choresRuleActive, clothing, clothingRuleActive, eatingOut, eatingOutRuleActive, essentials, essentialsRuleActive,
                food, foodRuleActive, fun, funRuleActive, gas, gasRuleActive, phone, phoneRuleActive, other, otherRuleActive, TimeSpan.Parse(startTimeRule),
                TimeSpan.Parse(endTimeRule), timeRuleActive, user.customerID);
                
            return RedirectToAction(nameof(Settings));
        }

        [Authorize]
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
