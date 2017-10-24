using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using Authorization.Models;
using Authorization.Extentions;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace Authorization.Pages.Settings.RoleModel
{
    public class Edit : BasePageModel
    {
		[BindProperty]
		public IList<Role> Roles { get; set; }
		public IList<Module> Modules { get; set; }
		public Edit(AppDbContext db) : base(db, "211")
        {
            this.Title = "Изменение ролей";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "..." });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Ролевая модель", NavigationUrl = "/settings/rolemodel/index" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Изменение ролей", NavigationUrl = "/settings/rolemodel/edit" });
        }
		public async Task<IActionResult> OnGetAsync()
		{
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            Roles = await _db.Roles.AsNoTracking().ToListAsync();
			Modules = await _db.Modules.AsNoTracking().ToListAsync();
			return Page();
		}

		[AjaxOnly]
		public async Task<IActionResult> OnPostAddRoleAsync()
		{
			if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
			{
				var dict = data.ToString().DeserializeAjaxString();

				var name = HttpUtility.UrlDecode(dict["name"]);

				if (!string.IsNullOrWhiteSpace(name))
				{
					var role = await _db.Roles.AddAsync(new Role() { Name = name });
					await _db.SaveChangesAsync();
					return new JsonResult(new { Status = "OK", Code = 200, Role = role.Entity });
				}
				return new JsonResult(data.ToString());
			}
			return new JsonResult(new { Status = "ERROR", Code = 500 });

		}

		[AjaxOnly]
		public async Task<IActionResult> OnPostDeleteRoleAsync()
		{
			if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
			{
				var id = HttpUtility.UrlDecode(data);
				if (!string.IsNullOrWhiteSpace(id))
				{
					if (int.TryParse(id, out int intId))
					{
						var role = await _db.Roles.FindAsync(intId);
						_db.Roles.Remove(role);
						await _db.SaveChangesAsync();
						return new JsonResult(new { Status = "OK", Code = 200 });
					}
				}
			}
			return new JsonResult(new { Status = "ERROR", Code = 500 });
		}

		[AjaxOnly]
		public async Task<IActionResult> OnPostEditRoleAsync()
		{
			if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
			{
				var dict = data.ToString().DeserializeAjaxString();
                var name = HttpUtility.UrlDecode(dict["name"]);
				var id = HttpUtility.UrlDecode(dict["id"]);
				if (!string.IsNullOrWhiteSpace(id))
				{
					if (int.TryParse(id, out int intId))
					{
						var role = await _db.Roles.FindAsync(intId);
						role.Name = name;
						_db.Attach(role).State = EntityState.Modified;
						await _db.SaveChangesAsync();
						return new JsonResult(new { Status = "OK", Code = 200, Role = role });
					}
				}
			}
			return new JsonResult(new { Status = "ERROR", Code = 500 });
		}

		[AjaxOnly]
		public async Task<IActionResult> OnPostAddModuleAsync()
		{
			if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
			{
				var dict = data.ToString().DeserializeAjaxString();
				var name = HttpUtility.UrlDecode(dict["name"]);
                var code = HttpUtility.UrlDecode(dict["code"]);
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(name))
				{
					var module = await _db.Modules.AddAsync(new Module() { Name = name, Code = code });
					await _db.SaveChangesAsync();
					return new JsonResult(new { Status = "OK", Code = 200, Module = module.Entity });
				}
				return new JsonResult(data.ToString());
			}
			return new JsonResult(new { Status = "ERROR", Code = 500 });

		}

		[AjaxOnly]
		public async Task<IActionResult> OnPostDeleteModuleAsync()
		{
			if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
			{
				var id = HttpUtility.UrlDecode(data);
				if (!string.IsNullOrWhiteSpace(id))
				{
					if (int.TryParse(id, out int intId))
					{
						var module = await _db.Modules.FindAsync(intId);
						_db.Modules.Remove(module);
						await _db.SaveChangesAsync();
						return new JsonResult(new { Status = "OK", Code = 200 });
					}
				}
			}
			return new JsonResult(new { Status = "ERROR", Code = 500 });
		}

		[AjaxOnly]
		public async Task<IActionResult> OnPostEditModuleAsync()
		{
			if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
			{
				var dict = data.ToString().DeserializeAjaxString();

				var name = HttpUtility.UrlDecode(dict["name"]);
                var code = HttpUtility.UrlDecode(dict["code"]);
                var id = HttpUtility.UrlDecode(dict["id"]);
				if (!string.IsNullOrWhiteSpace(id)
                    && !string.IsNullOrWhiteSpace(code)
                    && !string.IsNullOrWhiteSpace(name))
				{
					if (int.TryParse(id, out int intId))
					{
						var module = await _db.Modules.FindAsync(intId);
						module.Name = name;
                        module.Code = code;
                        _db.Attach(module).State = EntityState.Modified;
						await _db.SaveChangesAsync();
						return new JsonResult(new { Status = "OK", Code = 200, Module = module });
					}
				}
			}
			return new JsonResult(new { Status = "ERROR", Code = 500 });
		}
    }
}
