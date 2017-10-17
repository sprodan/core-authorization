using Authorization.Data;
using Authorization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Pages
{
    public class BasePageModel : PageModel
    {
        protected AppDbContext _db;
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string ActionUrl { get; set; }
        [BindProperty]
        public string ActionTitle { get; set; }
        [BindProperty]
        public Queue<Breadcrumb> Breadcrumbs { get; set; }
        [BindProperty]
        public string ModuleName { get; set; }
        [BindProperty]
        public IEnumerable<string> CurrentUserPermitions { get; }


        public BasePageModel(AppDbContext db, string moduleName)
        {
            _db = db;
            ModuleName = moduleName;
            //this.Request.Headers.TryGetValue("permitions", out StringValues permitions);
        }
    }
}
