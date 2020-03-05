using Headstone.Framework.SaaS.Models.Identity;
using System.Threading.Tasks;
#if NET452
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.Owin;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#endif
namespace Headstone.Framework.SaaS.Identity
{
#if NET452
    public class HeadstoneSignInManager : SignInManager<HeadstoneUser, int>
    {
        public HeadstoneSignInManager(HeadstoneUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(HeadstoneUser user)
        {
            return user.GenerateUserIdentityAsync((HeadstoneUserManager)UserManager);
        }

        public static HeadstoneSignInManager Create(
            IdentityFactoryOptions<HeadstoneSignInManager> options, IOwinContext context)
        {
            return new HeadstoneSignInManager(
                context.GetUserManager<HeadstoneUserManager>(), context.Authentication);
        }
    }
#elif NETCOREAPP2_2
    public class HeadstoneSignInManager : SignInManager<HeadstoneUser>
    {
        public HeadstoneSignInManager(UserManager<HeadstoneUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<HeadstoneUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<HeadstoneUser>> logger, IAuthenticationSchemeProvider schemes) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }
    }
#endif
}
