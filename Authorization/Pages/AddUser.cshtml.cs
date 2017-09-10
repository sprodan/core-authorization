using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;

namespace Authorization.Pages
{
    public class AddUserModel : PageModel
    {
        private AppDbContext _db;

        [BindProperty]
        public User User { get; set; }

        public AddUserModel(AppDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _db.Users.AddAsync(User);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}