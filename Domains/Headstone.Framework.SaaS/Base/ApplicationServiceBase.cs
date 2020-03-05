using Headstone.Framework.Data.Services;
using Headstone.Framework.SaaS.DataAccess;
using Headstone.Framework.SaaS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Base
{
    public class ApplicationServiceBase : EFServiceBase<Application, ApplicationDAO, SaasDbContext>
    {
    }
}
