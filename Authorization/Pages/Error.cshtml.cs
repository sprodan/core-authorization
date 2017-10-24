using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Pages
{
    public class Error : BasePageModel
    {
        public Error() : base(null, "10000")
        {
            this.Title = "Ошибка";
        }

        public void OnGet()
        {
            this.Request.Headers.TryGetValue("permitions", out StringValues permitions);
            Permitions = JsonConvert.DeserializeObject<IEnumerable<string>>(permitions);
        }
    }
}
