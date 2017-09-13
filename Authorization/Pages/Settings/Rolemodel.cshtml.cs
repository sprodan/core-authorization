﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorization.Pages.Settings
{
    public class RolemodelModel : BasePageModel
    {
		[BindProperty]
		public IList<Role> Roles { get; set; }
		public IList<Module> Modules { get; set; }
		public RolemodelModel(AppDbContext db) : base(db)
        {
            this.Title = "Изменение ролей";
            this.Breadcrumbs = new Queue<Breadcrumb>();
			Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
			Breadcrumbs.Enqueue(new Breadcrumb { Title = "Настройки", NavigationUrl = "/settings" });
		    Breadcrumbs.Enqueue(new Breadcrumb { Title = "Изменение ролей", NavigationUrl = "/settings/rolemodel" });
        }
		public async Task<IActionResult> OnGetAsync()
		{
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
				if (!string.IsNullOrWhiteSpace(name))
				{
					var module = await _db.Modules.AddAsync(new Module() { Name = name });
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
				var id = HttpUtility.UrlDecode(dict["id"]);
				if (!string.IsNullOrWhiteSpace(id))
				{
					if (int.TryParse(id, out int intId))
					{
						var module = await _db.Modules.FindAsync(intId);
						module.Name = name;
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