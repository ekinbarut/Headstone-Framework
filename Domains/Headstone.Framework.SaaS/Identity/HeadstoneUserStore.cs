using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Models.Identity;
using System;
#if NET452
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
#endif
namespace Headstone.Framework.SaaS.Identity
{
#if NET452
    public class HeadstoneUserStore : UserStore<HeadstoneUser, HeadstoneRole, int, HeadstoneUserLogin, HeadstoneUserRole, HeadstoneUserClaim>, 
                    IUserStore<HeadstoneUser,int>, IDisposable
#elif NETCOREAPP2_2
    public class HeadstoneUserStore : UserStore<HeadstoneUser, HeadstoneRole, SaasDbContext, int, HeadstoneUserClaim, HeadstoneUserRole, HeadstoneUserLogin, IdentityUserToken<int>, HeadstoneRoleClaim>,
                    IUserStore<HeadstoneUser>, IDisposable
#endif
    {
        public HeadstoneUserStore(SaasDbContext context) : base(context) { }
    }
}
