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
            return View(await _context.Notification.ToListAsync());
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
        
            
            return View("Index", await _context.Notification.ToListAsync());

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
