using Headstone.Framework.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.Models.Services
{
    public interface IServiceBase<R,T> 
        where T : class 
        where R : ServiceResponse<T>
    {
        R Create(T model);

        R Update(T model);

        R Delete(T model);

        R Delete(Expression<Func<T, bool>> predicate);

        R Find(Expression<Func<T, bool>> predicate);

        R FindIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        R FindIncluding(Expression<Func<T, bool>> predicate, params object[] includes);

        R Get(Expression<Func<T, bool>> predicate);

        R GetIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        R GetIncluding(Expression<Func<T, bool>> predicate, params object[] includes);

        R GetAll();

        R GetAllIncluding(params Expression<Func<T, object>>[] includes);

        R GetAllIncluding(params object[] includes);
    }
}
