using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBlog.Utility
{
    public class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
    {      
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Console.WriteLine(context);
        }
    }
}
