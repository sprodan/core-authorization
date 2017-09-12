using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using Microsoft.EntityFrameworkCore;
using Authorization.Extentions;
using System.IO;
using System.Text;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Web;

namespace Authorization.Pages.Settings
{
    public class Index : BasePageModel
    {
        [BindProperty]
        public IList<Role> Roles { get; set; }
        public Index(AppDbContext db) : base(db)
        {
            
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _db.Roles.AsNoTracking().ToListAsync();
            return Page();
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostAddRoleAsync()
        {
            if(Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();

                var name = HttpUtility.UrlDecode(dict["name"]);
                
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var role = await _db.Roles.AddAsync(new Role() { Name = name });
                    await _db.SaveChangesAsync();
                    return new JsonResult(new { Status = "OK", Code = 200, Role = role.Entity});
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
                if (!string.IsNullOrWhiteSpace(id)){
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
    }
}