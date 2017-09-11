using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Authorization.Models;
using System.Linq;

namespace Authorization.Pages
{
    public class IndexModel : BasePageModel 
    {
        public IndexModel(AppDbContext db) : base(db)
        {
            this.Title = "Главная страница";
            this.ActionTitle = "Тест";
            this.ActionUrl = "/";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "test", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "test", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "test", NavigationUrl = "/" });
        }

        public IList<User> Users { get; private set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
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
