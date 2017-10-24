using Authorization.Data;
using Authorization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
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
        public string ModuleCode { get; set; }
        protected string ActionCode { get; set; }
        [BindProperty]
        public bool ActionPermition { get; private set; }
        public IEnumerable<string> Permitions { get; set; }

        public BasePageModel(AppDbContext db, string moduleCode)
        {
            _db = db;
            ModuleCode = moduleCode;
        }

        protected bool CheckPermitions(IHeaderDictionary header)
        {
            this.Request.Headers.TryGetValue("permitions", out StringValues permitions);
            Permitions = JsonConvert.DeserializeObject<IEnumerable<string>>(permitions);
            ActionPermition = Permitions.Contains(ActionCode);
            return Permitions.Contains(ModuleCode);
        }
        protected bool CheckPermition(IHeaderDictionary header, string code)
        {
            this.Request.Headers.TryGetValue("permitions", out StringValues permitions);
            var permitionsList = JsonConvert.DeserializeObject<IEnumerable<string>>(permitions);
            return permitionsList.Contains(code);
        }


    }
}
