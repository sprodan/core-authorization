using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Pages
{
    public class IndexModel : PageModel
    {
        private AppDbContext _db;
        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public IList<Data.User> Users { get; private set; }


        public async Task<IActionResult> OnGetAsync()
        {
            if(Request.Cookies.TryGetValue("hash", out string hash))
            {
                Users = await _db.Users.AsNoTracking().ToListAsync();
                return Page();
            }
            return RedirectToPage("/User/Login");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);

            if(user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
