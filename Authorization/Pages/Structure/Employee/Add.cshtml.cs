using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Models;
using Microsoft.EntityFrameworkCore;
using Authorization.Extentions;

namespace Authorization.Pages.Structure.Employee
{
    public class AddModel : BasePageModel
    {
        public AddModel(AppDbContext db) : base(db, "311")
        {
            this.Title = "Сотрудники";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Сотрудники", NavigationUrl = "/structure/employee" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Добавить", NavigationUrl = "/structure/employee/add" });
        }

        [BindProperty]
        public List<Department> Departments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            await _db.Positions.ToListAsync();
            await _db.Teams.ToListAsync();
            Departments = await _db.Departments.ToListAsync();
            return Page();
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostAsync()
        {
            return null;
        }
    }
}