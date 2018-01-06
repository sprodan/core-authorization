using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Authorization.Data;
using Authorization.Extentions;
using Authorization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Authorization.Pages.Structure.Employee
{
    public class EditModel : BasePageModel
    {
        public EditModel(AppDbContext db) : base(db, "332")
        {
            this.Title = "Изменить сотрудника";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Сотрудники", NavigationUrl = "/structure/employee" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Изменить", NavigationUrl = "/structure/employee/edit" });
        }

        [BindProperty]
        public List<Department> Departments { get; set; }

        [BindProperty]
        public Data.Employee Employee { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            Departments = await _db.Departments.Include(d => d.Teams).Include(d => d.Positions).ToListAsync();
            var id = Request.Query.Keys.First();

            if (!int.TryParse(id, out int ide)) return Redirect("/error");
            
            Employee = await _db.Employees.FindAsync(ide);
            if (Employee == null) return Redirect("/error");
            return Page();
            
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();
                var id = HttpUtility.UrlDecode(dict["id"]);
                var name = HttpUtility.UrlDecode(dict["name"]);
                var surname = HttpUtility.UrlDecode(dict["surname"]);
                var patronymic = HttpUtility.UrlDecode(dict["patronymic"]);
                var positionId = HttpUtility.UrlDecode(dict["positionId"]);
                var teamId = HttpUtility.UrlDecode(dict["teamId"]);
                if (int.TryParse(id, out int idi) && int.TryParse(positionId, out int pid) && int.TryParse(teamId, out int tid))
                {
                    var employee = await _db.Employees.FindAsync(idi);
                    if (employee != null)
                    {
                        employee.Name = name;
                        employee.Surname = surname;
                        employee.Position = await _db.Positions.FindAsync(pid);
                        employee.Team = await _db.Teams.FindAsync(tid);
                        _db.Employees.Update(employee);
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Code = 200, Employee });
                    }
                }
            }
            return new JsonResult(new { Code = 500});
        }
    }
}