using Headstone.Framework.Data.Services;
using Headstone.Framework.Models;
using Headstone.Framework.Models.Responses;
using Headstone.Framework.Models.Services;
using Headstone.Framework.SaaS.DataAccess;
using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.SaaS.Base
{
    public class UserRoleServiceBase : IServiceBase<ServiceResponse<HeadstoneUserRole>, HeadstoneUserRole>
    {
        #region [ Implementation of IService ]

        public ServiceResponse<HeadstoneUserRole> Create(HeadstoneUserRole model)
        {
            using (var bo = new UserRoleDAO())
            {
                int result = bo.Insert(model as HeadstoneUserRole);
                if (result > 0)
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { model }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> Update(HeadstoneUserRole model)
        {
            using (var bo = new UserRoleDAO())
            {
                int result = bo.Update(model as HeadstoneUserRole);
                if (result > 0)
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { model }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> Delete(HeadstoneUserRole model)
        {
            using (var bo = new UserRoleDAO())
            {
                bool isDeleted = bo.Delete(model as HeadstoneUserRole, true);
                if (isDeleted)
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { model }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> Delete(Expression<Func<HeadstoneUserRole, bool>> predicate)
        {
            using (var bo = new UserRoleDAO())
            {
                var model = bo.Find(predicate);

                bool isDeleted = bo.Delete(model as HeadstoneUserRole, true);
                if (isDeleted)
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { model }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> Find(Expression<Func<HeadstoneUserRole, bool>> predicate)
        {
            using (var bo = new UserRoleDAO())
            {
                HeadstoneUserRole result = bo.Find(predicate) as HeadstoneUserRole;

                if (result != null)
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> FindIncluding(Expression<Func<HeadstoneUserRole, bool>> predicate, params Expression<Func<HeadstoneUserRole, object>>[] includes)
        {
            using (var bo = new UserRoleDAO())
            {
                var result = bo.Find(predicate, includes);

                if (result != null)
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> FindIncluding(Expression<Func<HeadstoneUserRole, bool>> predicate, params object[] includes)
        {
            using (var bo = new UserRoleDAO())
            {
                var result = bo.Find(predicate, includes);

                if (result != null)
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>() { }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> Get(Expression<Func<HeadstoneUserRole, bool>> predicate)
        {
            using (var bo = new UserRoleDAO())
            {
                var results = bo.GetList(predicate);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>()
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> GetIncluding(Expression<Func<HeadstoneUserRole, bool>> predicate, params Expression<Func<HeadstoneUserRole, object>>[] includes)
        {
            using (var bo = new UserRoleDAO())
            {
                var results = bo.GetList(predicate, includes);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>()
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> GetIncluding(Expression<Func<HeadstoneUserRole, bool>> predicate, params object[] includes)
        {
            using (var bo = new UserRoleDAO())
            {
                var results = bo.GetList(predicate, includes);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>()
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> GetAll()
        {
            using (var bo = new UserRoleDAO())
            {
                List<HeadstoneUserRole> result = bo.GetAll().ToList();
                if (result.Any())
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<HeadstoneUserRole>,

                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> GetAllIncluding(params Expression<Func<HeadstoneUserRole, object>>[] includes)
        {
            using (var bo = new UserRoleDAO())
            {
                var result = bo.GetAllIncluding(includes).ToList();

                if (result.Any())
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<HeadstoneUserRole>,

                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>(),
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUserRole> GetAllIncluding(params object[] includes)
        {
            using (var bo = new UserRoleDAO())
            {
                var result = bo.GetAll(includes).ToList();

                if (result.Any())
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<HeadstoneUserRole>,

                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUserRole>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUserRole>(),
                    };
                }
            }
        }

        #endregion
    }
}