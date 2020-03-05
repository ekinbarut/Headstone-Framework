using Headstone.Framework.Models.Services;
using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Models.Events;
using Headstone.Framework.SaaS.Models.Identity;
using Headstone.Framework.SaaS.Models.Requests;
using Headstone.Framework.SaaS.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Interfaces
{
    public interface IUserService
    {
        #region [ Queries ]

        UserServiceResponse<HeadstoneUser> GetUsers(UserQueryRequest req, List<ServiceLogRecord> logRecords = null);

        List<HeadstoneUserRole> GetRoles(int userId);

        #endregion

        #region [ Commands ]

        UserServiceResponse<HeadstoneUser> CreateUser(UserCreated ev, List<ServiceLogRecord> logRecords = null);

        #endregion
    }
}
