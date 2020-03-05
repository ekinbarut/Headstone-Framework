using Headstone.Framework.SaaS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Base;
using Headstone.Framework.SaaS.Models.Identity;
using Headstone.Framework.Models.Services;
using Headstone.Framework.SaaS.Models.Requests;
using Headstone.Framework.SaaS.Models.Responses;
using System.Diagnostics;
using Headstone.Framework.Models;
using System.Linq.Expressions;
using Headstone.Framework.Models.Responses;
using Headstone.Framework.SaaS.Models.Events;
using Headstone.Framework.SaaS.Identity;
#if NET452
using LinqKit;
#endif

namespace Headstone.Framework.SaaS
{
    public class UserService : IUserService
    {
        private HeadstoneUserManager userManager = new HeadstoneUserManager(new HeadstoneUserStore(SaasDbContext.Create()));
        private UserServiceBase userServiceBase = new UserServiceBase();
        private UserRoleServiceBase userRoleServiceBase = new UserRoleServiceBase();

        #region [ Queries ]

        public UserServiceResponse<HeadstoneUser> GetUsers(UserQueryRequest req, List<ServiceLogRecord> logRecords = null)
        {
            // Create the watch
            var sw = new Stopwatch();
            sw.Start();

            // Create a log record collection if necessary
            if (logRecords == null)
            {
                logRecords = new List<ServiceLogRecord>();
            }

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = "User query request received."
            });

            // Create response
            var response = new UserServiceResponse<HeadstoneUser>();

            #region [ Validate request ]

            // Check required data
            List<string> dataErrors = new List<string>();

            if (dataErrors.Count > 0)
            {
                // Add log
                logRecords.Add(new ServiceLogRecord()
                {
                    Type = "DEBUG",
                    TimeStamp = DateTime.Now,
                    Body = dataErrors.Count + " error(s) found within the posted data! Terminating the process. Errors:" + String.Join(";", dataErrors)
                });

                // Stop the sw
                sw.Stop();

                response.Type = ServiceResponseTypes.Error;
                response.Code = ((short)TenantServiceResponseCodes.Invalid_Request).ToString();
                response.PreProcessingTook = sw.ElapsedMilliseconds;
                response.Message = "There are some erros with the incoming request data!";
                response.Errors.AddRange(dataErrors);
                response.LogRecords = logRecords;

                return response;
            }

            #endregion

            // Stop the timer
            sw.Stop();

            // Set the pre-processing time and start the time
            response.PreProcessingTook = sw.ElapsedMilliseconds;
            sw.Start();

            #region [ Envelope settings ]

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = "Creating the envelope."
            });

            // Create the including fields according to the envelope
            var includes = new List<string>();
            if (!string.IsNullOrEmpty(req.Envelope))
            {
                if (req.Envelope == "full")
                {
                   
                }
            }

            #endregion

            #region [ Filters ]

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = "Creating filters."
            });
#if NET452
            // Check for filters
            Expression<Func<HeadstoneUser, bool>> filterPredicate = PredicateBuilder.New<HeadstoneUser>(true);

            // Add the filters
            if (req.UserIds.Any())
            {
                filterPredicate = filterPredicate.And(u => req.UserIds.Contains(u.Id));
            }

            if (req.Emails.Any())
            {
                filterPredicate = filterPredicate.And(u => req.Emails.Contains(u.Email));
            }

#endif
            #endregion

            #region [ Service call ]

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = "Calling the base service."
            });

            // Create a default list
            var baseServiceResponse = new ServiceResponse<HeadstoneUser>();
#if NET452
            // Make the query
            if (filterPredicate.Parameters.Count > 0)
            {
                baseServiceResponse = userServiceBase.GetIncluding(filterPredicate, includes.ToArray());
            }
            else
            {
                baseServiceResponse = userServiceBase.GetAllIncluding(includes.ToArray());
            }
#endif

            if (baseServiceResponse.Type != ServiceResponseTypes.Success)
            {
                // Add log
                logRecords.Add(new ServiceLogRecord()
                {
                    Type = "DEBUG",
                    TimeStamp = DateTime.Now,
                    Body = "There was an error while querying the users!"
                });

                // Stop the sw
                sw.Stop();

                response.Type = ServiceResponseTypes.Error;
                response.Code = ((short)TenantServiceResponseCodes.General_Exception).ToString();
                response.ServiceTook = sw.ElapsedMilliseconds;
                response.Message = "There was an error while querying the users!";
                response.Errors.Add("There was an error while querying the users!");
                response.LogRecords = logRecords;

                return response;
            }

            // Set the result
            response.Result = baseServiceResponse.Result;

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = "Users successfuly fetched."
            });

            // Stop the sw
            sw.Stop();

            response.Type = ServiceResponseTypes.Success;
            response.Code = ((short)TenantServiceResponseCodes.Request_Successfuly_Completed).ToString();
            response.ServiceTook = sw.ElapsedMilliseconds;
            response.Message = "Users successfuly fetched";
            response.LogRecords = logRecords;

            #endregion

            return response;
        }

        public List<HeadstoneUserRole> GetRoles(int userId)
        {
            var roles = userRoleServiceBase.Get(ur => ur.UserId == userId).Result;

            return roles;
        }

        #endregion

        #region [ Command ]

        public UserServiceResponse<HeadstoneUser> CreateUser(UserCreated ev, List<ServiceLogRecord> logRecords = null)
        {
            // Create the watch
            var sw = new Stopwatch();
            sw.Start();

            // Create a log record collection if necessary
            if (logRecords == null)
            {
                logRecords = new List<ServiceLogRecord>();
            }

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = "User creation request received."
            });

            // Create a response object
            var response = new UserServiceResponse<HeadstoneUser>();

            #region [ Validate request ]

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = "User has the required permissions. Now validating the incoming data."
            });

            // Check required data
            List<string> dataErrors = new List<string>();

            if (String.IsNullOrEmpty(ev.AppKey))
            {
                dataErrors.Add("No valid application key!");
            }

            if (String.IsNullOrEmpty(ev.Environment))
            {
                dataErrors.Add("No valid environment!");
            }

            if (dataErrors.Count > 0)
            {
                // Add log
                logRecords.Add(new ServiceLogRecord()
                {
                    Type = "DEBUG",
                    TimeStamp = DateTime.Now,
                    Body = dataErrors.Count + " error(s) found within the posted data! Terminating the process. Errors:" + String.Join(";", dataErrors)
                });

                // Stop the sw
                sw.Stop();

                response.Type = ServiceResponseTypes.Error;
                response.Code = ((short)TenantServiceResponseCodes.Invalid_Request).ToString();
                response.PreProcessingTook = sw.ElapsedMilliseconds;
                response.Message = "There are some erros with the incoming request data!";
                response.Errors.AddRange(dataErrors);
                response.LogRecords = logRecords;

                return response;
            }

            #endregion

            #region [ Data manuplation ]



            #endregion

            // Stop the timer
            sw.Stop();

            // Set the pre-processing time and start the time
            response.PreProcessingTook = sw.ElapsedMilliseconds;
            sw.Start();

            #region [ Create wallet ]

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = "Creating the user."
            });

            // Create the user
            var user = new HeadstoneUser()
            {
                Firstname = ev.Firstname,
                Lastname = ev.Lastname,
                Gender = ev.Gender,
                Birthdate = ev.DateOfBirth,
                Email = ev.Email,
                UserName = ev.Email,
                PhoneNumber = ev.PhoneNumber,
                MobileNumber = ev.MobileNumber,                
            };

            // Add log
            logRecords.Add(new ServiceLogRecord()
            {
                Type = "DEBUG",
                TimeStamp = DateTime.Now,
                Body = string.Format("User created. UserToken:{0}; SessionId:{1}", ev.UserToken, ev.SessionId)
            });

            #endregion

            #region [ Save user ]

            // Save the user
            var baseServiceResponse = userManager.CreateAsync(user).Result;

            if (!baseServiceResponse.Succeeded)
            {
                // Add log
                logRecords.Add(new ServiceLogRecord()
                {
                    Type = "ERROR",
                    TimeStamp = DateTime.Now,
                    Body = "There was an error while saving the user!"
                });

                // Stop the sw
                sw.Stop();

                response.Type = ServiceResponseTypes.Error;
                response.Code = ((short)TenantServiceResponseCodes.General_Exception).ToString();
                response.ServiceTook = sw.ElapsedMilliseconds;
                response.Message = "There was an error while creating the user!";
                response.Errors.Add("There was an error while creating the user!");
                response.LogRecords = logRecords;

                return response;

            }
            else
            {
                // Add log
                logRecords.Add(new ServiceLogRecord()
                {
                    Type = "DEBUG",
                    TimeStamp = DateTime.Now,
                    Body = string.Format("User successfuly created. UserId:{0}; UserToken:{1}; SessionId:{2}",
                                            user.Id, ev.UserToken, ev.SessionId)
                });

                // Add the new object to the result
                response.Result.Add(user);

                // Set the wallet id
                response.UserId = user.Id;
            }

            #endregion            

            // Stop the sw
            sw.Stop();

            response.Type = ServiceResponseTypes.Success;
            response.Code = ((short)TenantServiceResponseCodes.Request_Successfuly_Completed).ToString();
            response.ServiceTook = sw.ElapsedMilliseconds;
            response.Message = string.Format("User successfuly created. UserId:{0}; UserToken:{1}; SessionId:{2}",
                                            user.Id, ev.UserToken, ev.SessionId);
            response.LogRecords = logRecords;

            return response;
        }

        #endregion
    }
}
