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

namespace Authorization.Pages.Settings.Users
{
    public class AddModel : BasePageModel
    {
        public AddModel(AppDbContext db) : base(db, "221")
        {
            this.Title = "Добавить пользователя";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "..." });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Пользователи", NavigationUrl = "/settings/users/index" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Добавить", NavigationUrl = "/settings/users/add" });

        }

        [BindProperty]
        public List<Employee> Employees { get; set; }
        [BindProperty]
        public List<Role> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            Employees = await _db.Employees.ToListAsync();
            Roles = await _db.Roles.ToListAsync();
            return Page();
        }
        [AjaxOnly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();
                var login = HttpUtility.UrlDecode(dict["login"]);
                var password = HttpUtility.UrlDecode(dict["password"]);
                var roleId = HttpUtility.UrlDecode(dict["roleId"]);
                var employeeId = HttpUtility.UrlDecode(dict["employeeId"]);

                if (!string.IsNullOrWhiteSpace(login) && 
                    !string.IsNullOrWhiteSpace(password) &&
                    int.TryParse(roleId, out int iroleId) &&
                    int.TryParse(employeeId, out int iemployeeId))
                {
                    var role = await _db.Roles.FindAsync(iroleId);
                    var employee = await _db.Employees.FindAsync(iemployeeId);
                    var user = await _db.Users.AddAsync(
                        new Data.User()
                        {
                            Login = login,
                            IsActive = true,
                            Password = password,
                            Role = role,
                            Employee = employee
                        });
                    await _db.SaveChangesAsync();
                    return new JsonResult(new { Status = "OK", Code = 200, User = user.Entity });
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }
    }
}