using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Authorization.Extentions;
using Microsoft.Extensions.Primitives;
using System.Web;
using Authorization.Models;

namespace Authorization.Pages.Structure
{
    public class TeamsModel : BasePageModel
    {
        public TeamsModel(AppDbContext db) : base (db, "320")
        {
            this.Title = "Команды";
            this.ActionTitle = "Система грейдов";
            this.ActionUrl = "/structure/grades";
            this.ActionCode = "310";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Команды", NavigationUrl = "/structure/teams" });
        }
        [BindProperty]
        public List<Department> Departments { get; set; }
        [BindProperty]
        public bool ViewDepartmentsPermition { get; set; }
        [BindProperty]
        public bool EditDepartmentsPermition { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            ViewDepartmentsPermition = CheckPermition(this.Request.Headers, "311");
            EditDepartmentsPermition = CheckPermition(this.Request.Headers, "312");
            await _db.Teams.ToListAsync();
            Departments = await _db.Departments.ToListAsync();
            //var positions = await _db.Positions.ToListAsync();
            return Page();
        }
        
        [AjaxOnly]
        public async Task<IActionResult> OnGetJsonAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers))
                return new JsonResult(new { Code = "403", Message = "Forbidden"});
            var query = Request.QueryString;
            var dict = query.ToString().DeserializeAjaxString();
            if(dict.TryGetValue("departmentId", out string departmentId))
            {
                if (!string.IsNullOrWhiteSpace(departmentId))
                {
                    if (int.TryParse(departmentId, out int did))
                    {
                        await _db.Teams.ToListAsync();
                        var department = await _db.Departments.FindAsync(did);
                        return new JsonResult(new { Code = "200", Teams = department.Teams });
                    }
                }
            }
            else
            {
                var teams = await _db.Teams.ToListAsync();
                return new JsonResult(new { Code = "200", Teams = teams });
            }
            return new JsonResult(new { Code = "500", Message = "WrongData" });
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostDeleteTeamAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var teamId = data[0];
                if (!string.IsNullOrWhiteSpace(teamId))
                {
                    if (int.TryParse(teamId, out int tid))
                    {
                        var team = await _db.Teams.FindAsync(tid);
                        if (team != null) _db.Teams.Remove(team);
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Status = "OK", Code = 200, Team = team });
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }

        public async Task<IActionResult> OnPostAddTeamAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();

                var name = HttpUtility.UrlDecode(dict["name"]);
                var departmentId = HttpUtility.UrlDecode(dict["departmentId"]);

                if (!string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(departmentId))
                {
                    if (int.TryParse(departmentId, out int did))
                    {
                        var department = await _db.Departments.FindAsync(did);
                        var team = await _db.Teams.AddAsync(new Team() { Name = name, Department = department});
                        await _db.SaveChangesAsync();
                        return new JsonResult(new { Status = "OK", Code = 200, Position = team.Entity });
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }

        public async Task<IActionResult> OnPostEditTeamAsync()
        {
            if (Request.Form.TryGetValue("jsonRequest", out StringValues data))
            {
                var dict = data.ToString().DeserializeAjaxString();

                var name = HttpUtility.UrlDecode(dict["name"]);
                var teamId = HttpUtility.UrlDecode(dict["id"]);

                if (!string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(teamId))
                {
                    if (int.TryParse(teamId, out int tid))
                    {
                        var team = await _db.Teams.FindAsync(tid);
                        if (team != null)
                        {
                            team.Name = name;
                            await _db.SaveChangesAsync();
                            return new JsonResult(new { Status = "OK", Code = 200, Team = team });
                        }
                    }
                }
            }
            return new JsonResult(new { Status = "ERROR", Code = 500 });
        }
    }
}