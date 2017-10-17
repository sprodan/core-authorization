using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Models;
using Authorization.Data;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Pages.Settings.Users
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(AppDbContext db) : base(db, "users")
        {
            this.Title = "Пользователи";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Настройки" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Пользователи", NavigationUrl = "/settings/users/index" });
        }
        [BindProperty]
        public List<Models.User> Users { get; set; }
        public List<Role> Roles { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _db.Roles.AsNoTracking().ToListAsync();
            var users = await _db.Users.AsNoTracking().ToListAsync();
            Users = new List<Models.User>();
            foreach(var user in users)
            {
                var isOnline = false;
                var role = await _db.Roles.FindAsync(user.IdRole);
                var employee = await _db.Employees.FindAsync(user.IdEmployee);
                Users.Add(new Models.User()
                {
                    Id = user.Id,
                    Login = user.Login,
                    Name = employee?.Name,
                    Role = role,
                    IsOnline = isOnline //TODO: calculateOnline
                });
            }
            return Page();
        }
    }
}