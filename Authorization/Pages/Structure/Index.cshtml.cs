using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;
using Authorization.Models;

namespace Authorization.Pages.Structure
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(AppDbContext db) : base(db)
        {
            this.Title = "Структура компании";
            this.ActionTitle = "Система грейдов";
            this.ActionUrl = "/structure/grades";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Структура", NavigationUrl = "/structure/index" });
        }
        public void OnGet()
        {

        }
    }
}