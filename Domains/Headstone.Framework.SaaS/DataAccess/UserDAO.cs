using Headstone.Framework.Data.Channels;
using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.DataAccess
{
    public class UserDAO : EFDataChannel<HeadstoneUser, SaasDbContext>
    {
    }
}
