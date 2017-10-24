using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Models;
using Authorization.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Authorization.Pages.Settings.Users
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(AppDbContext db) : base(db, "220")
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
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            Roles = await _db.Roles.ToListAsync();
            var users = await _db.Users.ToListAsync();
            var employees = await _db.Employees.ToListAsync();
            Users = new List<Models.User>();
            foreach(var user in users)
            {
                var isOnline = false;
                var role = await _db.Roles.FindAsync(user.Role.Id);

                var employee = employees.Find(x => x.Id == user.Employee.Id);
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