using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CommerceBankProject.Areas.Identity.Data;
using CommerceBankProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CommerceBankProject.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CommerceBankDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            CommerceBankDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "First Name")]
            public string firstName { get; set; }
            [Display(Name = "Last Name")]
            public string lastName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var u = await _userManager.FindByIdAsync(user.Id);
            var userName = u.UserName;
            var phoneNumber = u.PhoneNumber;
            var first = u.firstName;
            var last = u.lastName;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                firstName = first,
                lastName = last
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var u = await _userManager.FindByIdAsync(user.Id);
            var phoneNumber = u.PhoneNumber;
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var first = u.firstName;
            var last = u.lastName;
            if (Input.firstName != first || Input.lastName != last)
            {
                string query = "Update [AspNetUsers] set firstName = {0}, lastName = {1} where Id = {2}";
                var result = await _context.Database.ExecuteSqlRawAsync(query, Input.firstName, Input.lastName, u.Id);
                if (result == 0)
                {
                    StatusMessage = "Unexpected error when trying to set customer name.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
