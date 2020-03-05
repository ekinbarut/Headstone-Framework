#if NET452
using Microsoft.AspNet.Identity.EntityFramework;
#elif NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;
#endif
namespace Headstone.Framework.SaaS.Models.Identity
{
    public class HeadstoneUserLogin : IdentityUserLogin<int>
    {
    }
}
