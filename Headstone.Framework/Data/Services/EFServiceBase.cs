using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Headstone.Framework.Data.Channels;
using Headstone.Framework.Models;
using Headstone.Framework.Models.Responses;
using Headstone.Framework.Models.Services;
#if NET452
using System.Data.Entity;
using System.Data.SqlClient;
#elif NETCOREAPP2_2
using Microsoft.EntityFrameworkCore;
#endif

namespace Headstone.Framework.Data.Services
{
    public class EFServiceBase<T, DAO, CTX> : IServiceBase<ServiceResponse<T>, T>
        where T : Entity
        where DAO : EFDataChannel<T, CTX>, new()
        where CTX : DbContext, new()
    {
        #region [ Implementation of IService ]

        public ServiceResponse<T> Create(T model)
        {
            using (var bo = new DAO())
            {
                // Set the missing fields
                model.Created = DateTime.UtcNow;

                //(model as T).Customer.ContactInformation.ForEach(ci => ci.);
                int result = bo.Insert(model as T);
                if (result > 0)
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { model }
                    };
                }
            }
        }

        public ServiceResponse<T> Update(T model)
        {
            using (var bo = new DAO())
            {
                int result = bo.Update(model as T);
                if (result > 0)
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { model }
                    };
                }
            }
        }

        public ServiceResponse<T> Delete(T model)
        {
            using (var bo = new DAO())
            {
                bool isDeleted = bo.Delete(model as T, true);
                if (isDeleted)
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { model }
                    };
                }
            }
        }

        public ServiceResponse<T> Delete(Expression<Func<T, bool>> predicate)
        {
            using (var bo = new DAO())
            {
                var model = bo.Find(predicate);

                bool isDeleted = bo.Delete(model as T, true);
                if (isDeleted)
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { model }
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { model }
                    };
                }
            }
        }

        public ServiceResponse<T> Find(Expression<Func<T, bool>> predicate)
        {
            using (var bo = new DAO())
            {
                T result = bo.Find(predicate) as T;

                if (result != null)
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL
                    };
                }
            }
        }

        public ServiceResponse<T> FindIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            using (var bo = new DAO())
            {
                var result = bo.Find(predicate, includes);

                if (result != null)
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { }
                    };
                }
            }
        }

        public ServiceResponse<T> FindIncluding(Expression<Func<T, bool>> predicate, params object[] includes)
        {
            using (var bo = new DAO())
            {
                var result = bo.Find(predicate, includes);

                if (result != null)
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { result }
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>() { }
                    };
                }
            }
        }

        public ServiceResponse<T> Get(Expression<Func<T, bool>> predicate)
        {
            using (var bo = new DAO())
            {
                var results = bo.GetList(predicate);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>()
                    };
                }
            }
        }

        public ServiceResponse<T> GetIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            using (var bo = new DAO())
            {
                var results = bo.GetList(predicate, includes);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>()
                    };
                }
            }
        }

        public ServiceResponse<T> GetIncluding(Expression<Func<T, bool>> predicate, params object[] includes)
        {
            using (var bo = new DAO())
            {
                var results = bo.GetList(predicate, includes);

                if (results != null && results.Any())
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>()
                    };
                }
            }
        }

        public ServiceResponse<T> GetAll()
        {
            using (var bo = new DAO())
            {
                List<T> result = bo.GetAll().ToList();
                if (result.Any())
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<T>,

                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL
                    };
                }
            }
        }

        public ServiceResponse<T> GetAllIncluding(params Expression<Func<T, object>>[] includes)
        {
            using (var bo = new DAO())
            {
                var result = bo.GetAllIncluding(includes).ToList();

                if (result.Any())
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<T>,

                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>(),
                    };
                }
            }
        }

        public ServiceResponse<T> GetAllIncluding(params object[] includes)
        {
            using (var bo = new DAO())
            {
                var result = bo.GetAll(includes).ToList();

                if (result.Any())
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = result as List<T>,

                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>(),
                    };
                }
            }
        }

        #endregion

        public ServiceResponse<T> Query<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> sortExpression, bool isDesc, int pageSize, int pageIndex, params object[] includes)
        {
            using (var bo = new DAO())
            {
                var results = bo.GetList(predicate, sortExpression, isDesc, pageSize, pageIndex, includes);

                var totalCount = bo.CountOfRecord(predicate);

                if (results != null)
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results,
                        TotalCount = totalCount,
                        CurrentPageIndex = pageIndex
                    };
                }
                else
                {
                    return new ServiceResponse<T>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<T>(),
                        TotalCount = 0,
                        CurrentPageIndex = pageIndex
                    };
                }
            }
        }

#if NET452
        public ServiceResponse<X> ExecuteSP<X>(string commandName, List<SqlParameter> parameters)
        {
            using (var bo = new DAO())
            {
                var results = bo.ExecuteDatatableAndParse<X>(commandName, parameters);

                if (results != null)
                {
                    return new ServiceResponse<X>()
                    {
                        Type = ServiceResponseTypes.Success,
                        Source = ServiceResponseSources.MsSQL,
                        Result = results,
                        TotalCount = results.Count,
                        CurrentPageIndex = 1
                    };
                }
                else
                {
                    return new ServiceResponse<X>()
                    {
                        Type = ServiceResponseTypes.Error,
                        Source = ServiceResponseSources.MsSQL,
                        Result = new List<X>(),
                        TotalCount = 0,
                        CurrentPageIndex = 0
                    };
                }
            }
        }
#endif
    }
}
