using Headstone.Framework.Models.Services;
using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Models.Requests;
using Headstone.Framework.SaaS.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Interfaces
{
    public interface ITenantService
    {
        #region [ Queries ]

        Tenant GetTenantById(int tenantId);

        Tenant GetTenantByDomainName(string domain);

        TenantServiceResponse<Tenant> GetTenants(TenantQueryRequest req, List<ServiceLogRecord> logRecords = null);

        TenantServiceResponse<Application> GetApplications(ApplicationQueryRequest req, List<ServiceLogRecord> logRecords = null);

        TenantServiceResponse<AccessKey> GetAccessKeys(AccessKeyQueryRequest req, List<ServiceLogRecord> logRecords = null);

        AccessKey GetAccessKey(string appkey);

        #endregion

    }
}
