using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#if NET452
using EntityFramework.Extensions;
using LinqKit;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
#elif NETCOREAPP2_2
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
#endif

namespace Headstone.Framework.Data.Channels
{
    public abstract class EFDataChannel<T, context> : IDisposable //TODO : Add log lines back!
        where T : class
        where context : DbContext, new()
    {
        #region [ Private Member(s) ]

        protected context Context { get; set; }

        private DbSet<T> dbset
        {
            get
            {
                return Context.Set<T>();
            }
        }

        protected Type CurrentType { get; set; }

        #endregion

        #region [ Constructor(s) ]

        public EFDataChannel()
        {
            Context = new context();
            this.CurrentType = typeof(T);
#if NET452
            Context.Configuration.ProxyCreationEnabled = false;
            Context.Configuration.LazyLoadingEnabled = false;
            LinqKitExtension.QueryOptimizer = ExpressionOptimizer.visit;
#endif
        }

        #endregion

#if NETCOREAPP2_2
        #region [ Override ]
        public int SaveChangesHelper()
        {
            var validationErrors = this.Context.ChangeTracker
                    .Entries<IValidatableObject>()
                    .SelectMany(e => e.Entity.Validate(null))
                    .Where(r => r != ValidationResult.Success);

            if (validationErrors.Any())
            {
                string finalMessage = string.Empty;
                foreach (var validationError in validationErrors)
                {
                    string message = validationError.ErrorMessage;
                    finalMessage += message + Environment.NewLine;
                }

                throw new Exception(finalMessage);
            }

            return this.Context.SaveChanges();
        }
        #endregion
#endif

        #region [ CRUD Actions ]

        /// <summary>
        /// Method to save t object in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public virtual int Insert(T entity)
        {
            try
            {
                this.dbset.Add(entity);
#if NET452
                return this.Context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException exc)
            {
                string finalMessage = string.Empty;
                foreach (var validationErrors in exc.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        finalMessage += message + Environment.NewLine;
                    }
                }
                //LogService.Error("Error on insert Entity Validation Error : " + finalMessage, exc);
                throw new Exception(finalMessage, exc);
            }
#elif NETCOREAPP2_2
                return SaveChangesHelper();
            }
#endif

            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on insert {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to delete t object from database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(T entity, bool permanently = false)
        {
            try
            {
#if NET452
                this.Context.Set(this.CurrentType).Attach(entity);

                if (permanently)
                {
                    this.Context.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
                }
#elif NETCOREAPP2_2
                this.Context.Set<T>().Attach(entity);
                if (permanently)
                {
                    this.Context.Entry<T>(entity).State = EntityState.Deleted;
                }
#endif
                dbset.Remove(entity);
                this.Context.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on delete {0}.", typeof(T).Name), exc);

            }
        }

        /// <summary>
        /// Method to delete with expression using EntityFramework Extended
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
#if NET452
        public virtual int Delete(Expression<Func<T, bool>> filterExpression, bool permanently = false)
        {

            return dbset.Where(filterExpression).Delete();
        }
#elif NETCOREAPP2_2
        public virtual void Delete(Expression<Func<T, bool>> filterExpression, bool permanently = false)
        {
            dbset.RemoveRange(dbset.Where(filterExpression));
        }
#endif

        /// <summary>
        /// Method to update t object in database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public virtual int Update(T entity)
        {
            try
            {
#if NET452
                this.Context.Set(this.CurrentType).Attach(entity);

                this.Context.Entry<T>(entity).State = EntityState.Modified;
                return this.Context.SaveChanges();
            }
            catch (DbEntityValidationException exc)
            {
                string finalMessage = string.Empty;
                foreach (var validationErrors in exc.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        finalMessage += message + Environment.NewLine;
                    }
                }
                //LogService.Error("Error on insert Entity Validation Error : " + finalMessage, exc);
                throw new Exception(finalMessage, exc);
            }
#elif NETCOREAPP2_2
                this.Context.Set<T>().Attach(entity);

                this.Context.Entry<T>(entity).State = EntityState.Modified;
                return SaveChangesHelper();
            }
#endif


            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on update {0}.", typeof(T).Name), exc);
            }
        }
#if NET452
#pragma warning disable CS0618 // 'BatchExtensions.Update<TEntity>(IQueryable<TEntity>, Expression<Func<TEntity, bool>>, Expression<Func<TEntity, TEntity>>)' is obsolete: 'The API was refactored to no longer need this extension method. Use query.Where(expression).Update(updateExpression) syntax instead.'
        /// <summary>
        /// Method to update with expression using EntityFramework Extended
        /// </summary>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public virtual int Update(Expression<Func<T, T>> updateExpression)
        {
            try
            {
                return dbset.Update(updateExpression);
            }
            catch (DbEntityValidationException exc)
            {
                string finalMessage = string.Empty;
                foreach (var validationErrors in exc.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        finalMessage += message + Environment.NewLine;
                    }
                }
                //LogService.Error("Error on insert Entity Validation Error : " + finalMessage, exc);
                throw new Exception(finalMessage, exc);
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on update {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to update with expression using EntityFramework Extended
        /// </summary>
        /// <param name="filterExpression"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        public virtual int Update(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression)
        {
            try
            {
                return dbset.Update(filterExpression, updateExpression);

            }
            catch (DbEntityValidationException exc)
            {
                string finalMessage = string.Empty;
                foreach (var validationErrors in exc.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        finalMessage += message + Environment.NewLine;
                    }
                }
                //LogService.Error("Error on insert Entity Validation Error : " + finalMessage, exc);
                throw new Exception(finalMessage, exc);
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on update {0}.", typeof(T).Name), exc);
            }
        }
#endif

#region [ Fetchs ]

#region [ Get All ]

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
#pragma warning restore CS0618 // 'BatchExtensions.Update<TEntity>(IQueryable<TEntity>, Expression<Func<TEntity, bool>>, Expression<Func<TEntity, TEntity>>)' is obsolete: 'The API was refactored to no longer need this extension method. Use query.Where(expression).Update(updateExpression) syntax instead.'
        {
            return this.dbset;
        }

        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbset;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual IQueryable<T> GetAll(params object[] includes)
        {
            try
            {
                IQueryable<T> query = dbset;// this.dbset;

                if (includes.Length > 0)
                {
                    query = query.Include(includes[0].ToString());
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i].ToString());
                    }
                }

                return query;

            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on find {0}.", typeof(T).Name), exc);
            }
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {

            try
            {
                IQueryable<T> query = dbset;// this.dbset;
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
                return query.Where(predicate);

            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on find {0}.", typeof(T).Name), exc);
            }
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, params object[] includes)
        {

            try
            {
                IQueryable<T> query = dbset;// this.dbset;

                if (includes.Length > 0)
                {
                    query = query.Include(includes[0].ToString());
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i].ToString());
                    }
                }

                return query.Where(predicate);

            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on find {0}.", typeof(T).Name), exc);
            }
        }

#endregion

#region [ Find ]

        /// <summary>
        /// Method to find first of entity via predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return dbset.FirstOrDefault(predicate);
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on find {0}.", typeof(T).Name), exc);
            }
        }

        public virtual T Find(params object[] keyValues)
        {
            return this.dbset.Find(keyValues);
        }

        public virtual T Find<TKey>(Expression<Func<T, TKey>> sortExpression, bool isDesc, Expression<Func<T, bool>> predicate)
        {
            try
            {
                if (isDesc)
                {
                    return dbset.OrderBy(sortExpression).FirstOrDefault(predicate);

                }
                else
                {
                    return dbset.OrderByDescending(sortExpression).FirstOrDefault(predicate);
                }
                //return this.dbset.FirstOrDefault(predicate) as T;
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on find {0}.", typeof(T).Name), exc);
            }
        }

        public virtual T Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = dbset;// this.dbset;
                if (includes.Length > 0)
                {
                    query = query.Include(includes[0]);
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i]);
                    }
                }

                return query.FirstOrDefault<T>(predicate);

            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on find {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to find first of entity via predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual T Find(Expression<Func<T, bool>> predicate, params object[] includes)
        {
            try
            {
                IQueryable<T> query = dbset;// this.dbset;
                //IQueryable<T> query =  this.dbset;
                if (includes.Length > 0)
                {
                    query = query.Include(includes[0].ToString());
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i].ToString());
                    }
                }

                return query.FirstOrDefault<T>(predicate);

            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on find {0}.", typeof(T).Name), exc);
            }
        }

#endregion

#region [ Get List ]

        /// <summary>
        /// Method to get list of t object from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public virtual List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            try
            {
#if NET452
                return this.dbset.AsExpandable().Where(predicate).ToList<T>();
#elif NETCOREAPP2_2
                return this.dbset.Where(predicate).ToList<T>();
#endif
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public async virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
#if NET452
                return await this.dbset.AsExpandable().Where(predicate).ToListAsync<T>();
#elif NETCOREAPP2_2
                return await this.dbset.Where(predicate).ToListAsync<T>();
#endif
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public virtual List<T> GetList(Expression<Func<T, bool>> predicate, params object[] includes)
        {
            try
            {
                IQueryable<T> query = this.dbset;
                if (includes.Length > 0)
                {
                    query = query.Include(includes[0].ToString());
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i].ToString());
                    }
                }
#if NET452
                 return query.AsExpandable().Where(predicate).ToList<T>();
#elif NETCOREAPP2_2
                return query.Where(predicate).ToList<T>();
#endif

            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public async virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, params object[] includes)
        {
            try
            {
                IQueryable<T> query = this.dbset;
                if (includes.Length > 0)
                {
                    query = query.Include(includes[0].ToString());
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i].ToString());
                    }
                }
#if NET452
                return await query.AsExpandable().Where(predicate).ToListAsync<T>();
#elif NETCOREAPP2_2
                return await query.Where(predicate).ToListAsync<T>();
#endif
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public virtual List<T> GetList<TKey>(Expression<Func<T, TKey>> sortExpression)
        {
            try
            {
                return this.dbset.OrderByDescending(sortExpression).ToList<T>();
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public virtual List<T> GetList<TKey>(Expression<Func<T, TKey>> sortExpression, params object[] includes)
        {
            try
            {
                IQueryable<T> query = this.dbset;
                if (includes.Length > 0)
                {
                    query = query.Include(includes[0].ToString());
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i].ToString());
                    }
                }


                return query.OrderByDescending(sortExpression).ToList<T>();
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database by paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        public virtual List<T> GetList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> sortExpression, params object[] includes)
        {
            try
            {
                IQueryable<T> query = this.dbset;
                if (includes.Length > 0)
                {
                    query = query.Include(includes[0].ToString());
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i].ToString());
                    }
                }
#if NET452
                return query.AsExpandable().Where(predicate).OrderByDescending(sortExpression).ToList<T>();
#elif NETCOREAPP2_2
                return query.Where(predicate).OrderByDescending(sortExpression).ToList<T>();
#endif
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database by paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        public virtual List<T> GetList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> sortExpression, int pageSize, int pageIndex)
        {
            try
            {
#if NET452
                return this.dbset.AsExpandable().Where(predicate).OrderByDescending(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();

#elif NETCOREAPP2_2

                return this.dbset.Where(predicate).OrderByDescending(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();
#endif
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database by paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        public virtual List<T> GetList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> sortExpression, bool isDesc, int pageSize, int pageIndex, params object[] includes)
        {
            try
            {
                IQueryable<T> query = this.dbset;
                if (includes.Length > 0)
                {
                    query = query.Include(includes[0].ToString());
                    for (int i = 1; i < includes.Length; i++)
                    {
                        query = query.Include(includes[i].ToString());
                    }
                }
                if (isDesc)
                {
#if NET452
                    return query.AsExpandable().Where(predicate).OrderByDescending(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();
#elif NETCOREAPP2_2
                    return query.Where(predicate).OrderByDescending(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();
#endif
                }
                else
                {
#if NET452
                    return query.AsExpandable().Where(predicate).OrderBy(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();

#elif NETCOREAPP2_2
                    return query.Where(predicate).OrderBy(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();

#endif

                }
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get list of t object from database by paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        public virtual List<T> GetList<TKey>(Expression<Func<T, TKey>> sortExpression, bool isDesc, int pageSize, int pageIndex)
        {
            try
            {
                if (isDesc)
                {
#if NET452
                    return this.dbset.AsExpandable().OrderByDescending(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();

#elif NETCOREAPP2_2
                    return this.dbset.OrderByDescending(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();

#endif
                }
                else
                {
#if NET452
                    return this.dbset.AsExpandable().OrderBy(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();

#elif NETCOREAPP2_2
                    return this.dbset.OrderBy(sortExpression).Skip(pageSize * pageIndex).Take(pageSize).ToList<T>();

#endif
                }

            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get list of {0}.", typeof(T).Name), exc);
            }

        }

#endregion

#endregion

#region [ Scalars ]

        public virtual int CountOfRecord()
        {
            try
            {
#if NET452
                return this.dbset.AsExpandable().Count();
#elif NETCOREAPP2_2
                return this.dbset.Count();
#endif
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get count of {0}.", typeof(T).Name), exc);
            }
        }

        public virtual bool Exist(Expression<Func<T, bool>> predicate)
        {
            try
            {
#if NET452
                return dbset.AsExpandable().Any(predicate);
#elif NETCOREAPP2_2
                return dbset.Any(predicate);
#endif
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on checking if data exists: {0}.", typeof(T).Name), exc);
            }
        }

        /// <summary>
        /// Method to get count of records. 
        /// This count may use for paging
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual int CountOfRecord(Expression<Func<T, bool>> predicate)
        {
            try
            {
#if NET452
                return this.dbset.AsExpandable().Count(predicate);
#elif NETCOREAPP2_2
                return this.dbset.Count(predicate);
#endif
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Error on get count of {0}.", typeof(T).Name), exc);
            }
        }

        public virtual TResult GetMax<TResult>(Expression<Func<T, TResult>> selector)
        {
#if NET452
            return dbset.AsExpandable().Max(selector);
#elif NETCOREAPP2_2
            return dbset.Max(selector);
#endif
        }

        #endregion

        #endregion

        #region [ Dispose ]

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

#endregion

#if NET452
#region [ MS SQL Server Only ]
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
#pragma warning disable CS0168 // The variable 'ex' is declared but never used

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object ExecuteNonQuery(string commandName, List<SqlParameter> parameters)
        {
            // Create a new connection to the database
            SqlConnection connection = new SqlConnection(new context().Database.Connection.ConnectionString);

            // Open the connection
            connection.Open();

            // Create a new sql command object
            SqlCommand command = new SqlCommand(commandName, connection);

            // Tell the command that this is a stored procedure! 
            command.CommandType = CommandType.StoredProcedure;

            // Assign the parameters collection
            SqlParameter returnParam = null;

            if (parameters != null && parameters.Any())
            {
                //command.Parameters.AddRange(parameters);
                //returnParam = parameters.Where(a => a.Direction == ParameterDirection.Output).FirstOrDefault();

                foreach (SqlParameter param in parameters)
                {
                    command.Parameters.Add(param);

                    // Mark the return parameter
                    if (param.Direction == ParameterDirection.Output)
                    {
                        returnParam = command.Parameters[param.ParameterName];
                    }
                }
            }

            try
            {
                // Execute the command
                command.ExecuteNonQuery();

                // Get the return value
                return returnParam == null ? null : returnParam.Value;
            }
            catch (Exception ex)
            {
                //
                // TODO: Add logging here
                //
                throw;
            }
            finally
            {
                // Close the connection
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected int ExecuteCount(string commandName, params SqlParameter[] parameters)
        {
            // Create a new connection to the database
            SqlConnection connection = new SqlConnection(new context().Database.Connection.ConnectionString);

            // Open the connection
            connection.Open();

            // Create a new sql command object
            SqlCommand command = new SqlCommand(commandName, connection);

            // Tell the command that this is a stored procedure! 
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
                //foreach (SqlParameter param in parameters)
                //{
                //    command.Parameters.Add(param);
                //}
            }

            try
            {
                // Execute the command
                object result = command.ExecuteScalar();

                // Check the object
                if (result == DBNull.Value) return 0;

                // Get the return value
                return Convert.ToInt32(result);
            }
            catch
            {
                //
                // TODO: Add logging here
                //

                throw;
            }
            finally
            {
                // Close the connection
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object ExecuteObject(string commandName, List<SqlParameter> parameters)
        {
            // Create a new connection to the database
            SqlConnection connection = new SqlConnection(new context().Database.Connection.ConnectionString);

            // Open the connection
            connection.Open();

            // Create a new sql command object
            SqlCommand command = new SqlCommand(commandName, connection);

            // Tell the command that this is a stored procedure! 
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }

            try
            {
                // Execute the command
                object result = command.ExecuteScalar();

                // Get the return value
                return result;
            }
            catch
            {
                //
                // TODO: Add logging here
                //

                throw;
            }
            finally
            {
                // Close the connection
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataTable ExecuteDatatable(string commandName, List<SqlParameter> parameters)
        {
            // Create a new connection to the database
            SqlConnection connection = new SqlConnection(new context().Database.Connection.ConnectionString);

            // Open the connection
            connection.Open();

            // Create a new sql command object
            SqlCommand command = new SqlCommand(commandName, connection);

            // Tell the command that this is a stored procedure! 
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (SqlParameter param in parameters)
                {
                    command.Parameters.Add(param);
                }
            }

            try
            {
                // Creates an adapter object and fill datatable object
                using (SqlDataAdapter _adapterObject = new SqlDataAdapter(command))
                {
                    DataTable dt = new DataTable();
                    _adapterObject.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                // Close the connection
                connection.Close();
                throw;
            }
            finally
            {
                // Close the connection
                connection.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual List<X> ExecuteDatatableAndParse<X>(string commandName, List<SqlParameter> parameters)
        {
            try
            {
                return Context.Database.SqlQuery<X>(commandName, parameters.ToArray()).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }

#endregion
#endif
    }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
}
