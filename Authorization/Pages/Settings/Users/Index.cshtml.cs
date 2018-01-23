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
using Authorization.Extentions;
using System.Web;
using Microsoft.Extensions.Primitives;

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
            this.ActionCode = "221";
            this.ActionTitle = "Добавить";
            this.ActionUrl = "/settings/users/add";
        }
        [BindProperty]
        public List<Models.User> Users { get; set; }
        public List<Role> Roles { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            Roles = await _db.Roles.ToListAsync();
            var employees = await _db.Employees.ToListAsync();
            var users = await _db.Users.ToListAsync();
            Users = new List<Models.User>();
            foreach(var user in users)
            {
                var isOnline = false;
                if (!user.IsActive) continue;
                var userModel = new Models.User()
                {
                    Id = user.Id,
                    Login = user.Login,
                    IsOnline = isOnline //TODO: calculateOnline
                };
                if (user.Employee != null)
                {
                    var employee = employees.Find(x => x.Id == user.Employee.Id);
                    userModel.Name = $"{employee?.Name} {employee?.Surname}";
                }
                if (user.Role != null)
                {
                    var role = await _db.Roles.FindAsync(user.Role.Id);
                    userModel.Role = role;
                }
                Users.Add(userModel);
            }
            return Page();
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostBlockUserAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();
                var id = HttpUtility.UrlDecode(dict["id_user"]);

                if (int.TryParse(id, out var idUser))
                {
                    var user = await _db.Users.FindAsync(idUser);
                    if(user != null)
                    {
                        user.IsActive = false;
                    }
                    await _db.SaveChangesAsync();
                    return new JsonResult(new { Status = "OK", Code = 200, User = user });
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }
        [AjaxOnly]
        public async Task<IActionResult> OnPostEditRoleAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();
                var idu = HttpUtility.UrlDecode(dict["id_user"]);
                var idr = HttpUtility.UrlDecode(dict["id_role"]);
                if (int.TryParse(idu, out var idUser) &&
                    int.TryParse(idr, out var idRole))
                {
                    var user = await _db.Users.FindAsync(idUser);
                    var role = await _db.Roles.FindAsync(idRole);
                    if (user != null)
                    {
                        user.Role = role;
                    }
                    await _db.SaveChangesAsync();
                    return new JsonResult(new { Status = "OK", Code = 200, User = user });
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }
    }
}