#if NETCOREAPP2_2
using Microsoft.AspNetCore.Identity;

namespace Headstone.Framework.SaaS.Models.Identity
{
    public class HeadstoneRoleClaim : IdentityRoleClaim<int>
    {
    }
}
#endif