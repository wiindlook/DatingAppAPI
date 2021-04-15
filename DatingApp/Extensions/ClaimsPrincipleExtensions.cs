using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user) 
        {
           return user.FindFirst(ClaimTypes.NameIdentifier)?.Value; //pentru a nu mai scrie asta mereu si arata ok
        }
    }
}
