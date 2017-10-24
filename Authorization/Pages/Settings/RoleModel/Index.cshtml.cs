using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using Microsoft.EntityFrameworkCore;
using Authorization.Models;
using Authorization.Extentions;
using System.IO;
using System.Text;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Web;
using System.Diagnostics;

namespace Authorization.Pages.Settings.RoleModel
{
    public class Index : BasePageModel
    {
        [BindProperty]
        public IList<Role> Roles { get; set; }
        public IList<Module> Modules { get; set; }

        public IList<RoleModule> RoleModules { get; set; }
        public Index(AppDbContext db) : base(db, "210")
        {
            this.Title = "Ролевая модель";
            this.ActionTitle = "Изменение ролей";
            this.ActionUrl = "/settings/rolemodel/edit";
            this.ActionCode = "211";
			this.Breadcrumbs = new Queue<Breadcrumb>();
			Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
			Breadcrumbs.Enqueue(new Breadcrumb { Title = "Настройки" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Ролевая модель", NavigationUrl= "/settings/rolemodel/index" });
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            Roles = await _db.Roles.ToListAsync();
            Modules = await _db.Modules.ToListAsync();
            RoleModules = await _db.RoleModules.ToListAsync();
            return Page();
        }
        [AjaxOnly]
        public async Task<IActionResult> OnPostEditRoleModuleAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();

                if(!int.TryParse(HttpUtility.UrlDecode(dict["idrole"]), out int idRole)) return new JsonResult(new { Status = "ERROR", Code = 500 });
                if(!int.TryParse(HttpUtility.UrlDecode(dict["idmodule"]), out int idModule)) return new JsonResult(new { Status = "ERROR", Code = 500 });
                var isChecked = HttpUtility.UrlDecode(dict["checked"]);
                if(bool.TryParse(isChecked, out bool c))
                {
                    var role = await _db.Roles.FindAsync(idRole);
                    if (role == null) return new JsonResult(new { Status = "ERROR", Code = 500 });
                    var module = await _db.Modules.FindAsync(idModule);
                    if(module == null) return new JsonResult(new { Status = "ERROR", Code = 500 });
                    //var rolemodule = await _db.RoleModules.Where(x => x.RoleId == role.Id && x.ModuleId == module.Id).FirstOrDefaultAsync();
                    var rolemodule = await _db.RoleModules.Where(x => x.Role.Id == role.Id && x.Module.Id == module.Id).FirstOrDefaultAsync();

                    if (c)
                    {
                        if(rolemodule == null)
                        {
                            await _db.RoleModules.AddAsync(new RoleModule { Role = role, Module = module });
                        }
                    }
                    else
                    {
                        if (rolemodule != null)
                            _db.Remove(rolemodule);
                    }
                    try
                    {
                        await _db.SaveChangesAsync();
                    }catch(Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    
                    return new JsonResult(new { Status = "OK", Code = 200 });
                }
                //if (!string.IsNullOrWhiteSpace(name))
                //{
                //    var role = await _db.Roles.AddAsync(new Role() { Name = name });
                //    await _db.SaveChangesAsync();
                //    return new JsonResult(new { Status = "OK", Code = 200, Role = role.Entity });
                //}
                return new JsonResult(data.ToString());
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });

            return null;
        }

    }
}