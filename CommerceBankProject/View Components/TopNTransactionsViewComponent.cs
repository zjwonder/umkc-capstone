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
	public class TopNTransactionsViewComponent : ViewComponent
	{
		private readonly CommerceBankDbContext _context;

		public TopNTransactionsViewComponent(CommerceBankDbContext context)
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
								filter_trans.ID
								,filter_trans.customerID
								,filter_trans.actID
								,filter_trans.actType
								,filter_trans.onDate
								,filter_trans.balance
								,filter_trans.transType
								,filter_trans.amount
								,filter_trans.[description]
								,filter_trans.userEntered
								,filter_trans.category
							FROM
							(
								SELECT
									*
									,ROW_NUMBER() OVER(PARTITION BY trans.customerID ORDER BY trans.onDate DESC) RN
								FROM
									[CommerceBankProject].[dbo].[Transaction] trans
								WHERE 1=1
									AND trans.customerID = {0}
							) filter_trans
							WHERE 1=1
								AND filter_trans.RN <= 5";
			List<Transaction> tList = await _context.Transaction.FromSqlRaw(tQuery, user.customerID).ToListAsync();
			string actQuery = "Select distinct actID, actType from [Transaction] where customerID = {0}";
			List<AccountRecord> actList = await _context.Account.FromSqlRaw(actQuery, user.customerID).ToListAsync();
			string dateQuery = "Select top 1 onDate from [Transaction] where customerID = {0} order by ID";
			DateRecord record = await _context.Date.FromSqlRaw(dateQuery, user.customerID).FirstOrDefaultAsync();
			DateTime fromDate = record.onDate;
			record = await _context.Date.FromSqlRaw(dateQuery + " desc", user.customerID).FirstOrDefaultAsync();
			DateTime toDate = record.onDate;
			TIndexViewModel vmod = new TIndexViewModel(tList, actList, fromDate, toDate);

			return View(vmod);
		}
	}
}
