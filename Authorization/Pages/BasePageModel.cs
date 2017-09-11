using Authorization.Data;
using Authorization.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public BasePageModel(AppDbContext db)
        {
            _db = db;
        }
    }
}
