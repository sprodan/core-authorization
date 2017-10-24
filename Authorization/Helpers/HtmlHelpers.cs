using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsSelected(string moduleCode, params string[] pageCodes)
        {
            return pageCodes.Contains(moduleCode) ? "active" : "";
        }
    }
}
