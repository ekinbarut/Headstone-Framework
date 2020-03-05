using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Models
{
    public enum AccountStatus
    {
        Deleted = -99,

        Freezed = -1,

        Passive = 0,

        Active = 1,

        Verified = 2
    }

    // ACCESS
    public enum AccessPermissions
    {
        Read = 1,

        Write = 2,

        Full = 3
    }

    // MISC
    public enum TagType
    {
        General = 0,

        Attribute = 1,

        Badge = 2,

        Group = 3,

        Administrative = 99
    }

    // RESPONSE CODES
    public enum TenantServiceResponseCodes
    {
        Cancelled = -999,

        General_Exception = -300,

        Invalid_Request = -301,

        Authorization_Failed = -304,

        Invalid_Request_Parameters = -305,

        Request_Completed_WithErrors = -200,

        Request_Successfuly_Completed = 200
    }

    public enum PackageServiceResponseCodes
    {
        Cancelled = -999,

        General_Exception = -300,

        Invalid_Request = -301,

        Authorization_Failed = -304,

        Invalid_Request_Parameters = -305,

        Request_Completed_WithErrors = -200,

        Request_Successfuly_Completed = 200
    }
}
