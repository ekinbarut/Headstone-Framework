#if NET452
using Microsoft.AspNet.Identity.EntityFramework;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;
#endif

namespace Headstone.Framework.SaaS.Models.Identity
{
#if NET452
    public class HeadstoneRole : IdentityRole<int,HeadstoneUserRole>
    {
    }
#elif NETCOREAPP2_2
    public class HeadstoneRole : IdentityRole<int>
    {
    }
#endif
}
