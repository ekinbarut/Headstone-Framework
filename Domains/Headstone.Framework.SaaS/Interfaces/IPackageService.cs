using Headstone.Framework.Models.Services;
using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Models.Requests;
using Headstone.Framework.SaaS.Models.Responses;
using System.Collections.Generic;

namespace Headstone.Framework.SaaS.Interfaces
{
    public interface IPackageService
    {
        #region [ Queries ]

        PackageServiceResponse<Package> GetPackages(PackageQueryRequest req, List<ServiceLogRecord> logRecords = null);

        #endregion

    }
}
