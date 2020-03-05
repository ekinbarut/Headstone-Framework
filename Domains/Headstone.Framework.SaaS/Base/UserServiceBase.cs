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
    public class UserServiceBase : IServiceBase<ServiceResponse<HeadstoneUser>, HeadstoneUser>
    {
        #region [ Implementation of IService ]

        public ServiceResponse<HeadstoneUser> Create(HeadstoneUser model)
        {
            using (var bo = new UserDAO())
            {
                int result = bo.Insert(model as HeadstoneUser);
                if (result > 0)
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { model }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> Update(HeadstoneUser model)
        {
            using (var bo = new UserDAO())
            {
                int result = bo.Update(model as HeadstoneUser);
                if (result > 0)
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { model }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> Delete(HeadstoneUser model)
        {
            using (var bo = new UserDAO())
            {
                bool isDeleted = bo.Delete(model as HeadstoneUser, true);
                if (isDeleted)
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { model }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> Delete(Expression<Func<HeadstoneUser, bool>> predicate)
        {
            using (var bo = new UserDAO())
            {
                var model = bo.Find(predicate);

                bool isDeleted = bo.Delete(model as HeadstoneUser, true);
                if (isDeleted)
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { model }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> Find(Expression<Func<HeadstoneUser, bool>> predicate)
        {
            using (var bo = new UserDAO())
            {
                HeadstoneUser result = bo.Find(predicate) as HeadstoneUser;

                if (result != null)
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> FindIncluding(Expression<Func<HeadstoneUser, bool>> predicate, params Expression<Func<HeadstoneUser, object>>[] includes)
        {
            using (var bo = new UserDAO())
            {
                var result = bo.Find(predicate, includes);

                if (result != null)
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> FindIncluding(Expression<Func<HeadstoneUser, bool>> predicate, params object[] includes)
        {
            using (var bo = new UserDAO())
            {
                var result = bo.Find(predicate, includes);

                if (result != null)
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>() { }
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> Get(Expression<Func<HeadstoneUser, bool>> predicate)
        {
            using (var bo = new UserDAO())
            {
                var results = bo.GetList(predicate);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>()
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> GetIncluding(Expression<Func<HeadstoneUser, bool>> predicate, params Expression<Func<HeadstoneUser, object>>[] includes)
        {
            using (var bo = new UserDAO())
            {
                var results = bo.GetList(predicate, includes);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>()
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> GetIncluding(Expression<Func<HeadstoneUser, bool>> predicate, params object[] includes)
        {
            using (var bo = new UserDAO())
            {
                var results = bo.GetList(predicate, includes);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>()
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> GetAll()
        {
            using (var bo = new UserDAO())
            {
                List<HeadstoneUser> result = bo.GetAll().ToList();
                if (result.Any())
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<HeadstoneUser>,

                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> GetAllIncluding(params Expression<Func<HeadstoneUser, object>>[] includes)
        {
            using (var bo = new UserDAO())
            {
                var result = bo.GetAllIncluding(includes).ToList();

                if (result.Any())
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<HeadstoneUser>,

                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>(),
                    };
                }
            }
        }

        public ServiceResponse<HeadstoneUser> GetAllIncluding(params object[] includes)
        {
            using (var bo = new UserDAO())
            {
                var result = bo.GetAll(includes).ToList();

                if (result.Any())
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<HeadstoneUser>,

                    };
                }
                else
                {
                    return new ServiceResponse<HeadstoneUser>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<HeadstoneUser>(),
                    };
                }
            }
        }

        #endregion
    }
}