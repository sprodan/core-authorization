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

        //[AjaxOnly]
        public async Task<IActionResult> OnPostAddRoleAsync()
        {
            if(Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var keyValuePairs = data.ToString().Split('&')
                    .Select(value => value.Split('='))
                    .ToDictionary(pair => pair[0], pair => pair[1]);

                var name = HttpUtility.UrlDecode(keyValuePairs["name"]);
                
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
    }
}