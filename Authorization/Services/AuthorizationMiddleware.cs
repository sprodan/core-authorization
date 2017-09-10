using Authorization.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

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
            var db = context.RequestServices.GetService<AppDbContext>(); 
            var auth = await db.Authorizations.FirstOrDefaultAsync(x => x.Token == Guid.Parse(hash));
            if (auth == null)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }
            await _next.Invoke(context);
        }
    }

}
