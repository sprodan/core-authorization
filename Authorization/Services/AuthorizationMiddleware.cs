using Authorization.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Authorization.Models;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;

namespace Authorization.Services
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var timer = new Stopwatch();
            timer.Start();
            if (context.Request.Path == "/Auth/Login")
            {
                await _next.Invoke(context);
                return;
            }
            if (!context.Request.Cookies.TryGetValue("hash", out string hash))
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }
            var cache = context.RequestServices.GetService<Cache.Cache>();
            if (cache.Authorizations == null)
            {
                var db = context.RequestServices.GetService<AppDbContext>();
                await db.RoleModules.Include(rm => rm.Role).Include(rm => rm.Module).ToListAsync();
                cache.Authorizations = await db.Authorizations.Include(x => x.User).ToListAsync();
            }
            var auth = cache.Authorizations.FirstOrDefault(x => x.Token == Guid.Parse(hash));
            if (auth == null)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }
            

            if (auth.User.Role != null)
            {
                var permitions = auth.User.Role.RoleModules.Select(x => x.Module.Code);
                context.Request.Headers.Add("permitions", (StringValues)JsonConvert.SerializeObject(permitions));
            }
            timer.Stop();
            Console.WriteLine($"Role-Modules timer:  {timer.ElapsedMilliseconds}");
            await _next.Invoke(context);
        }
    }
}
