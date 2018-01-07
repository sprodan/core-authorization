using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Data;
using Authorization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Pages.Settings.Users
{
    public class AddModel : BasePageModel
    {
        public AddModel(AppDbContext db) : base(db, "221")
        {
            this.Title = "Добавить пользователя";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Настройки" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Пользователи", NavigationUrl = "/settings/users/index" });
        }

        [BindProperty]
        public List<Employee> Employees { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            Employees = await _db.Employees.ToListAsync();
            return Page();
        }
    }
}