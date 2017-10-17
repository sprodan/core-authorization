using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Data;

namespace Authorization.Pages.Structure
{
    public class TeamsModel : BasePageModel
    {
        public TeamsModel(AppDbContext db) : base (db, "structureTeams")
        {
            this.Title = "Команды";
        }
        public void OnGet()
        {

        }
    }
}