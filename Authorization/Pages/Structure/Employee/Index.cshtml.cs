using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Models;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Pages.Structure.Employee
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(AppDbContext db) : base(db, "330")
        {
            this.Title = "Сотрудники";
            this.ActionTitle = "Добавить сотрудника";
            this.ActionUrl = "/structure/employee/add";
            this.ActionCode = "331";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Сотрудники", NavigationUrl = "/structure/employee" });
        }

        [BindProperty]
        public List<Department> Departments { get; set; }
        [BindProperty]
        public List<Position> Positions { get; set; }
        [BindProperty]
        public List<Team> Teams { get; set; }
        [BindProperty]
        public List<Data.Employee> Employees { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            Positions = await _db.Positions.ToListAsync();
            Teams = await _db.Teams.ToListAsync();
            Departments = await _db.Departments.ToListAsync();
            await _db.Photos.ToListAsync();
            Employees = await _db.Employees.ToListAsync();
            return Page();
        }
    }
}