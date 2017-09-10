using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _db;

        public LoginModel(AppDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Data.User User { get; set; }

        public async void OnGetAsync()
        {
            if (Request.Cookies.TryGetValue("hash", out string hash))
            {
                Response.Cookies.Delete("hash");
                var auth = await _db.Authorizations.FirstOrDefaultAsync(x => x.Token == Guid.Parse(hash));
                if (auth != null) _db.Authorizations.Remove(auth);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            var user = _db.Users.FirstOrDefault(x => x.Login == User.Login && x.Password == User.Password);
            if (user == null) return Page();

            var authorization = _db.Authorizations.FirstOrDefault(x => x.User.Id == user.Id);
            Guid guid;
            if (authorization == null)
            {
                guid = Guid.NewGuid();
                await _db.Authorizations.AddAsync(new Data.Authorization()
                {
                    DateTime = DateTime.Now,
                    Expiration = new TimeSpan(0, 1, 0, 0),
                    Token = guid,
                    User = user
                });
            }
            else
            {
                guid = authorization.Token;
                authorization.DateTime = DateTime.Now;
                _db.Attach(authorization).State = EntityState.Modified;
                
            }
            await _db.SaveChangesAsync();
            Response.Cookies.Append("hash", guid.ToString(), new CookieOptions() { Expires = DateTime.Now.AddDays(1) });

            return RedirectToPage("/Index");
        }
    }
}