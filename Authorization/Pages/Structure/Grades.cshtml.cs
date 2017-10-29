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
using Microsoft.CodeAnalysis;

namespace Authorization.Pages.Structure
{
    public class GradesModel : BasePageModel
    {
        public GradesModel(AppDbContext db) : base(db, "310")
        {
            this.Title = "Система грейдов";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Структура", NavigationUrl = "/structure/index" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Система грейдов", NavigationUrl = "/structure/grades" });

        }

        [BindProperty]
        public List<Department> Departments { get; set; }
        [BindProperty]
        public bool ViewDepartmentsPermition { get; set; }
        [BindProperty]
        public bool EditDepartmentsPermition { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            ViewDepartmentsPermition = CheckPermition(this.Request.Headers, "311");
            EditDepartmentsPermition = CheckPermition(this.Request.Headers, "312");
            Departments = await _db.Departments.Include(d => d.Positions).ToListAsync();
            if (Departments == null) return Page(); //Todo make empty field
            Departments.ForEach(x =>
            {
                x.Positions = x.Positions.OrderBy(p => p.Grade).ToList();
            });
            return Page();
        }

        public async Task<IActionResult> OnGetJsonAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers))
                return new JsonResult(new { Code = "403", Message = "Forbidden" });
            var query = Request.QueryString;
            var dict = query.ToString().DeserializeAjaxString();
            if (dict.TryGetValue("departmentId", out string departmentId))
            {
                if (!string.IsNullOrWhiteSpace(departmentId))
                {
                    if (int.TryParse(departmentId, out int did))
                    {
                        await _db.Positions.ToListAsync();
                        var department = await _db.Departments.FindAsync(did);
                        return new JsonResult(new { Code = "200", Positions = department.Positions });
                    }
                }
            }
            else
            {
                var positions = await _db.Positions.ToListAsync();
                return new JsonResult(new { Code = "200", Positions = positions });
            }
            return new JsonResult(new { Code = "500", Message = "WrongData" });
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostAddDepartmentAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();

                var name = HttpUtility.UrlDecode(dict["name"]);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    var department = await _db.Departments.AddAsync(new Department() { Name = name});
                    await _db.SaveChangesAsync();
                    return new JsonResult(new { Status = "OK", Code = 200, Department = department.Entity });
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }
        [AjaxOnly]
        public async Task<IActionResult> OnPostEditDepartmentAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();
                var id = HttpUtility.UrlDecode(dict["id"]);
                var name = HttpUtility.UrlDecode(dict["name"]);

                if (!string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(id) &&
                    int.TryParse(id, out int iddep))
                {
                    var department = await _db.Departments.FindAsync(iddep);
                    if(department != null)
                    {
                        department.Name = name;
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Status = "OK", Code = 200, Department = department });
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostDeleteDepartmentAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();
                var id = HttpUtility.UrlDecode(dict["id"]);
                
                if (!string.IsNullOrWhiteSpace(id) &&
                    int.TryParse(id, out int iddep))
                {
                    var department = await _db.Departments.FindAsync(iddep);
                    if (department != null)
                    {
                        _db.Departments.Remove(department);
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Status = "OK", Code = 200, Department = department });
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostDeletePositionAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var positionId = data[0];
                if (!string.IsNullOrWhiteSpace(positionId))
                {
                    if (int.TryParse(positionId, out int pid))
                    {
                        var position = await _db.Positions.FindAsync(pid);
                        if (position != null) _db.Positions.Remove(position);
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Status = "OK", Code = 200, Position = position});
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
                        var department = await _db.Departments.FindAsync(did);
                        var position = await _db.Positions.AddAsync(new Position() { Name = name, Department = department, Grade = g });
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Status = "OK", Code = 200, Position = position.Entity });
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }

        public async Task<IActionResult> OnPostEditPositionAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();

                var name = HttpUtility.UrlDecode(dict["name"]);
                var positionId = HttpUtility.UrlDecode(dict["id"]);
                var grade = HttpUtility.UrlDecode(dict["grade"]);

                if (!string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(positionId) &&
                    !string.IsNullOrWhiteSpace(grade))
                {
                    if (int.TryParse(positionId, out int pid) && int.TryParse(grade, out int g))
                    {
                        var position = await _db.Positions.FindAsync(pid);
                        if(position != null)
                        {
                            position.Grade = g;
                            position.Name = name;
                            await _db.SaveChangesAsync();
                            return new JsonResult(new { Status = "OK", Code = 200, Position = position });
                        }
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }
    }
}