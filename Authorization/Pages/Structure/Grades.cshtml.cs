using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using Authorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Authorization.Extentions;
using System.Web;

namespace Authorization.Pages.Structure
{
    public class GradesModel : BasePageModel
    {
        public GradesModel(AppDbContext db) : base(db)
        {
            this.Title = "Система грейдов";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Структура", NavigationUrl = "/structure/index" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Система грейдов", NavigationUrl = "/structure/grades" });

        }

        [BindProperty]
        public List<Department> Departments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Departments = await _db.Departments.AsNoTracking().ToListAsync();
            if (Departments == null) return Page(); //Todo make empty field
            foreach (var d in Departments)
            {
                d.Positions = await _db.Positions.Where(x => x.IdDepartment == d.Id).ToListAsync();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeletePositionAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();
                var positionId = HttpUtility.UrlDecode(dict["positionId"]);
                if (!string.IsNullOrWhiteSpace(positionId))
                {
                    if (int.TryParse(positionId, out int pid))
                    {
                        var position = await _db.Positions.FindAsync(pid);
                        if (position != null) _db.Positions.Remove(position);
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Status = "OK", Code = 200});
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }

        public async Task<IActionResult> OnPostAddPositionAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();

                var name = HttpUtility.UrlDecode(dict["name"]);
                var departmentId = HttpUtility.UrlDecode(dict["departmentId"]);
                var grade = HttpUtility.UrlDecode(dict["grade"]);

                if (!string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(departmentId) &&
                    !string.IsNullOrWhiteSpace(grade))
                {
                    if(int.TryParse(departmentId, out int did) && int.TryParse(grade, out int g))
                    {
                        var position = await _db.Positions.AddAsync(new Position() { Name = name, IdDepartment = did, Grade = g });
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Status = "OK", Code = 200, Position = position.Entity });
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }
    }
}