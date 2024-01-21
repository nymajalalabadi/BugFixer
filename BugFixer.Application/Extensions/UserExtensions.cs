using BugFixer.domain.Entities.Account;
using System.Security.Claims;
using System.Security.Principal;

namespace BugFixer.Application.Extensions
{
    public static class UserExtensions
    {
        public static long GetUserId(this ClaimsPrincipal claims)
        {
            if (claims != null)
            {
                var data = claims.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (data != null)
                    return Convert.ToInt64(data.Value);
            }
            return default(long);
        }

        public static long GetUserId(this IPrincipal principal)
        {
            var user = (ClaimsPrincipal)principal;

            return user.GetUserId();
        }

        public static string GetUserDisplayName(this User user)
        {
            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.FirstName} {user.LastName}";
            }

            var email = user.Email.Split("@")[0];

            return email;
        }
    }
}
