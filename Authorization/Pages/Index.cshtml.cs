using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Authorization.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Authorization.Pages
{
    public class IndexModel : BasePageModel 
    {
        public IndexModel(AppDbContext db) : base(db, "100")
        {
            var req = Request;
            this.Title = "Главная страница";
            this.ActionTitle = "Тест";
            this.ActionUrl = "/";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "test", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "test", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "test", NavigationUrl = "/" });
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            return Page();
        }
        
    }
}
