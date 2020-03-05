using System;
using System.Security.Claims;
using System.Threading.Tasks;
#if NET452
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;
#endif

namespace Headstone.Framework.SaaS.Models.Identity
{
#if NET452
    public class HeadstoneUser : IdentityUser<int,HeadstoneUserLogin, HeadstoneUserRole, HeadstoneUserClaim>
#elif NETCOREAPP2_2
    public class HeadstoneUser : IdentityUser<int>
#endif
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Gender { get; set; }

        public DateTime? Birthdate { get; set; }

        public string MobileNumber { get; set; }

        public bool? MobileNumberConfirmed { get; set; }
#if NET452
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<HeadstoneUser, int> manager)
        {
            // Note the authenticationType must match the one defined in 
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here 
            return userIdentity;
        }
#elif NETCOREAPP2_2
        public async Task<IdentityResult> GenerateUserIdentityAsync(UserManager<HeadstoneUser> manager)
        {
            // Note the authenticationType must match the one defined in 
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateAsync(this);

            // Add custom user claims here 
            return userIdentity;
        }
#endif
    }
}