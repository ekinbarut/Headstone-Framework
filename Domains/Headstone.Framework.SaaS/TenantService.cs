using Headstone.Framework.SaaS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Base;
using Headstone.Framework.SaaS.Models.Responses;
using Headstone.Framework.SaaS.Models.Requests;
using Headstone.Framework.Models.Services;
using System.Diagnostics;
using Headstone.Framework.Models;
using System.Linq.Expressions;
using Headstone.Framework.Models.Responses;
#if NET452
using LinqKit;
#endif

namespace Headstone.Framework.SaaS
{
    public class TenantService : ITenantService
    {
        private TenantServiceBase tenantServiceBase = new TenantServiceBase();
        private ApplicationServiceBase applicationServiceBase = new ApplicationServiceBase();
        private AccessKeyServiceBase accessKeyServiceBase = new AccessKeyServiceBase();

        #region [ Queries ]

        public Tenant GetTenantById(int tenantId)
        {
            var tenant = tenantServiceBase.GetIncluding(t => t.TenantId == tenantId, "Applications.Properties", "Properties").Result.FirstOrDefault();

            return tenant;
        }

        public Tenant GetTenantByDomainName(string domain)
        {
            throw new NotImplementedException();
        }

        public TenantServiceResponse<Tenant> GetTenants(TenantQueryRequest req, List<ServiceLogRecord> logRecords = null)
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
                Body = "Tenant query request received."
            });

            // Create response
            var response = new TenantServiceResponse<Tenant>();

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
            includes.Add("Properties");
            if (!string.IsNullOrEmpty(req.Envelope))
            {
                if (req.Envelope == "full")
                {
                    includes.Add("Applications");
                    includes.Add("PaymentInformation");
                    includes.Add("BillingInformation");
                    includes.Add("ContactInformation");
                    includes.Add("Subscriptions");
                    includes.Add("Users");
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
            Expression<Func<Tenant, bool>> filterPredicate = PredicateBuilder.New<Tenant>(true);

            // Add the filters
            if (req.TenantIds.Any())
            {
                filterPredicate = filterPredicate.And(t => req.TenantIds.Contains(t.TenantId));
            }

            if (req.ApplicationIds.Any())
            {
                filterPredicate = filterPredicate.And(t => req.ApplicationIds.Intersect(t.Applications.Select(a => a.Id)).Any());
            }

            if (req.Status.Any())
            {
                filterPredicate = filterPredicate.And(t => req.Status.Contains(t.Status));
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
            var baseServiceResponse = new ServiceResponse<Tenant>();
#if NET452
            // Make the query
            if (filterPredicate.Parameters.Count > 0)
            {
                baseServiceResponse = tenantServiceBase.GetIncluding(filterPredicate, includes.ToArray());
            }
            else
            {
                baseServiceResponse = tenantServiceBase.GetAllIncluding(includes.ToArray());
            }
#endif

            if (baseServiceResponse.Type != ServiceResponseTypes.Success)
            {
                // Add log
                logRecords.Add(new ServiceLogRecord()
                {
                    Type = "DEBUG",
                    TimeStamp = DateTime.Now,
                    Body = "There was an error while querying the tenants!"
                });

                // Stop the sw
                sw.Stop();

                response.Type = ServiceResponseTypes.Error;
                response.Code = ((short)TenantServiceResponseCodes.General_Exception).ToString();
                response.ServiceTook = sw.ElapsedMilliseconds;
                response.Message = "There was an error while querying the tenants!";
                response.Errors.Add("There was an error while querying the tenants!");
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
                Body = "Tenants successfuly fetched."
            });

            // Stop the sw
            sw.Stop();

            response.Type = ServiceResponseTypes.Success;
            response.Code = ((short)TenantServiceResponseCodes.Request_Successfuly_Completed).ToString();
            response.ServiceTook = sw.ElapsedMilliseconds;
            response.Message = "Tenants successfuly fetched";
            response.LogRecords = logRecords;

            #endregion

            return response;
        }

        public TenantServiceResponse<Application> GetApplications(ApplicationQueryRequest req, List<ServiceLogRecord> logRecords = null)
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
                Body = "Application query request received."
            });

            // Create response
            var response = new TenantServiceResponse<Application>();

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
            includes.Add("Properties");
            if (!string.IsNullOrEmpty(req.Envelope))
            {
                if (req.Envelope == "full")
                {
                    includes.Add("Parent");
                    includes.Add("Children");
                    includes.Add("AccessKeys");
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
            Expression<Func<Application, bool>> filterPredicate = PredicateBuilder.New<Application>(true);

            // Add the filters
            if (req.TenantId.HasValue)
            {
                filterPredicate = filterPredicate.And(a => req.TenantId == a.TenantId);
            }

            if (req.ApplicationIds.Any())
            {
                filterPredicate = filterPredicate.And(a => req.ApplicationIds.Contains(a.Id));
            }

            if (req.Status.Any())
            {
                filterPredicate = filterPredicate.And(a => req.Status.Contains(a.Status));
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
            var baseServiceResponse = new ServiceResponse<Application>();
#if NET452
            // Make the query
            if (filterPredicate.Parameters.Count > 0)
            {
                baseServiceResponse = applicationServiceBase.GetIncluding(filterPredicate, includes.ToArray());
            }
            else
            {
                baseServiceResponse = applicationServiceBase.GetAllIncluding(includes.ToArray());
            }
#endif

            if (baseServiceResponse.Type != ServiceResponseTypes.Success)
            {
                // Add log
                logRecords.Add(new ServiceLogRecord()
                {
                    Type = "DEBUG",
                    TimeStamp = DateTime.Now,
                    Body = "There was an error while querying the applications!"
                });

                // Stop the sw
                sw.Stop();

                response.Type = ServiceResponseTypes.Error;
                response.Code = ((short)TenantServiceResponseCodes.General_Exception).ToString();
                response.ServiceTook = sw.ElapsedMilliseconds;
                response.Message = "There was an error while querying the applications!";
                response.Errors.Add("There was an error while querying the applications!");
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
                Body = "Applications successfuly fetched."
            });

            // Stop the sw
            sw.Stop();

            response.Type = ServiceResponseTypes.Success;
            response.Code = ((short)TenantServiceResponseCodes.Request_Successfuly_Completed).ToString();
            response.ServiceTook = sw.ElapsedMilliseconds;
            response.Message = "Applications successfuly fetched";
            response.LogRecords = logRecords;

            #endregion

            return response;
        }

        public TenantServiceResponse<AccessKey> GetAccessKeys(AccessKeyQueryRequest req, List<ServiceLogRecord> logRecords = null)
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
                Body = "Access key query request received."
            });

            // Create response
            var response = new TenantServiceResponse<AccessKey>();

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
            Expression<Func<AccessKey, bool>> filterPredicate = PredicateBuilder.New<AccessKey>(true);

            // Add the filters
            if (req.TenantId.HasValue)
            {
                filterPredicate = filterPredicate.And(ak => req.TenantId == ak.Application.TenantId);
            }

            if (req.ApplicationId.HasValue)
            {
                filterPredicate = filterPredicate.And(ak => req.ApplicationId == ak.ApplicationId);
            }

            if (req.Status.Any())
            {
                filterPredicate = filterPredicate.And(ak => req.Status.Contains(ak.Status));
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
            var baseServiceResponse = new ServiceResponse<AccessKey>();
#if NET452
            // Make the query
            if (filterPredicate.Parameters.Count > 0)
            {
                baseServiceResponse = accessKeyServiceBase.GetIncluding(filterPredicate, includes.ToArray());
            }
            else
            {
                baseServiceResponse = accessKeyServiceBase.GetAllIncluding(includes.ToArray());
            }
#endif

            if (baseServiceResponse.Type != ServiceResponseTypes.Success)
            {
                // Add log
                logRecords.Add(new ServiceLogRecord()
                {
                    Type = "DEBUG",
                    TimeStamp = DateTime.Now,
                    Body = "There was an error while querying the access keys!"
                });

                // Stop the sw
                sw.Stop();

                response.Type = ServiceResponseTypes.Error;
                response.Code = ((short)TenantServiceResponseCodes.General_Exception).ToString();
                response.ServiceTook = sw.ElapsedMilliseconds;
                response.Message = "There was an error while querying the access keys!";
                response.Errors.Add("There was an error while querying the access keys!");
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
                Body = "Access keys successfuly fetched."
            });

            // Stop the sw
            sw.Stop();

            response.Type = ServiceResponseTypes.Success;
            response.Code = ((short)TenantServiceResponseCodes.Request_Successfuly_Completed).ToString();
            response.ServiceTook = sw.ElapsedMilliseconds;
            response.Message = "Access keys successfuly fetched";
            response.LogRecords = logRecords;

            #endregion

            return response;
        }

        public AccessKey GetAccessKey(string appKey)
        {
            var accesKey = accessKeyServiceBase.GetIncluding(a => a.Key == appKey, "Application.Properties", "Application.Tenant").Result.FirstOrDefault();

            return accesKey;
        }

        #endregion

    }
}
