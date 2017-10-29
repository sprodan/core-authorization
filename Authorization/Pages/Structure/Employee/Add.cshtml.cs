using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Authorization.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Authorization.Models;
using Microsoft.EntityFrameworkCore;
using Authorization.Extentions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Authorization.Pages.Structure.Employee
{
    public class AddModel : BasePageModel
    {
        private IHostingEnvironment _environment;
        public AddModel(AppDbContext db, IHostingEnvironment env) : base(db, "331")
        {
            _environment = env;
            this.Title = "Сотрудники";
            this.Breadcrumbs = new Queue<Breadcrumb>();
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Главная", NavigationUrl = "/" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Сотрудники", NavigationUrl = "/structure/employee" });
            Breadcrumbs.Enqueue(new Breadcrumb { Title = "Добавить", NavigationUrl = "/structure/employee/add" });
        }

        [BindProperty]
        public List<Department> Departments { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!base.CheckPermitions(this.Request.Headers)) return Redirect("/error");
            await _db.Positions.ToListAsync();
            await _db.Teams.ToListAsync();
            Departments = await _db.Departments.ToListAsync();
            return Page();
        }

        [AjaxOnly]
        public async Task<IActionResult> OnPostAsync()
        {
            if (Request.Form.TryGetValue("name", out StringValues name) &&
                Request.Form.TryGetValue("surname", out StringValues surname) &&
                Request.Form.TryGetValue("positionId", out StringValues positionId) &&
                Request.Form.TryGetValue("teamId", out StringValues teamId)
                )
            {
                var photo = await UploadAvatar(Request.Form.Files[0]);
                if(!int.TryParse(positionId.ToString(), out int pid) || !int.TryParse(teamId, out int tid)) return new JsonResult(new { Code = 500 });
                var employee = new Data.Employee()
                {
                    Name = name.ToString(),
                    Surname = surname.ToString(),
                    Photo = photo,
                    Position = await _db.Positions.FindAsync(pid),
                    Team = await _db.Teams.FindAsync(tid)
                };
                if (Request.Form.TryGetValue("patronymic", out StringValues patronymic))
                {
                    //todo add patronym to db;
                }
                var e = await _db.Employees.AddAsync(employee);
                await _db.SaveChangesAsync();
                return new JsonResult(new {Code = 200, Employee = e.Entity});
            }
            return new JsonResult(new {Code = 500});
            
        }

        private async Task<Photo> UploadAvatar(IFormFile file)
        {
            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('\"');
            var fileExtention = filename.Substring(filename.Length - 3);
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

            filename = _environment.WebRootPath + $@"\uploaded_files\avatars\{timestamp}.{fileExtention}";
            using (FileStream fs = System.IO.File.Create(filename))
            {
                await file.CopyToAsync(fs);
                fs.Flush();
            }
            var photo = await _db.Photos.AddAsync(new Photo() { Url = $@"\uploaded_files\avatars\{timestamp}.{fileExtention}" });
            await _db.SaveChangesAsync();
            return photo.Entity;
        }
    }
}