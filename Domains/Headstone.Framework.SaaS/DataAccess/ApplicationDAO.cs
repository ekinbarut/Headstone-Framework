﻿using Headstone.Framework.Data.Channels;
using Headstone.Framework.SaaS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.DataAccess
{
    public class ApplicationDAO : EFDataChannel<Application, SaasDbContext>
    {
    }
}
