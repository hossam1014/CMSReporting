using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var nameIdentifierClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim != null && int.TryParse(nameIdentifierClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}
