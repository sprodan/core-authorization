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

namespace Authorization.Pages.Settings
{
    public class Index : BasePageModel
    {
        [BindProperty]
        public IList<Role> Roles { get; set; }
        public IList<Module> Modules { get; set; }
        public Index(AppDbContext db) : base(db)
        {
            this.Title = "Настройки";
            this.ActionTitle = "Изменение ролей";
            this.ActionUrl = "/settings/rolemodel";
			this.Breadcrumbs = new Queue<Breadcrumb>();
			Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
			Breadcrumbs.Enqueue(new Breadcrumb { Title = "Настройки", NavigationUrl = "/settings" });
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _db.Roles.AsNoTracking().ToListAsync();
            Modules = await _db.Modules.AsNoTracking().ToListAsync();
            return Page();
        }

    }
}