using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CommerceBankProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CommerceBankProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CommerceBankProject.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly CommerceBankDbContext _context;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, CommerceBankDbContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            string userID = user.Id;
            string style = HttpContext.Session.GetString("UserStyle");
            string darkQuery = "Update [AspNetUsers] set darkMode = {0} where Id = {1}";
            if (style == "dark" && !user.darkMode)
            {
                await _context.Database.ExecuteSqlRawAsync(darkQuery, 1, userID);
            }
            else if (style == "light" && user.darkMode)
            {
                await _context.Database.ExecuteSqlRawAsync(darkQuery, 0, userID);
            }
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (style != null) { 
                HttpContext.Session.SetString("UserStyle", style);
            }
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
