using Headstone.Framework.Models;
using Headstone.Framework.Models.Responses;
using Headstone.Framework.Models.Services;
using Headstone.Framework.SaaS.Base;
using Headstone.Framework.SaaS.Interfaces;
using Headstone.Framework.SaaS.Models;
using Headstone.Framework.SaaS.Models.Requests;
using Headstone.Framework.SaaS.Models.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
#if NET452
using LinqKit;
#elif NETCOREAPP2_2

#endif

namespace Headstone.Framework.SaaS
{
    public class PackageService : IPackageService
    {
        private TenantServiceBase tenantServiceBase = new TenantServiceBase();
        private ApplicationServiceBase applicationServiceBase = new ApplicationServiceBase();
        private AccessKeyServiceBase accessKeyServiceBase = new AccessKeyServiceBase();
        private PackageServiceBase packageServiceBase = new PackageServiceBase();

        #region [ Queries ]

        public PackageServiceResponse<Package> GetPackages(PackageQueryRequest req, List<ServiceLogRecord> logRecords = null)
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
                Body = "Package query request received."
            });

            // Create response
            var response = new PackageServiceResponse<Package>();

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
            includes.Add("Tags");
            if (!string.IsNullOrEmpty(req.Envelope))
            {
                if (req.Envelope == "full")
                {
                    includes.Add("Features");
                    includes.Add("Restrictions");

                }
                else if (req.Envelope == "subs")
                {
                    includes.Add("Features");
                    includes.Add("Restrictions");
                    includes.Add("Subscriptions");
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
            Expression<Func<Package, bool>> filterPredicate = PredicateBuilder.New<Package>(true);

            // Add the filters
            if (req.TenantIds.Any())
            {
                filterPredicate = filterPredicate.And(p => req.TenantIds.Contains(p.TenantId));
            }

            if (req.ApplicationIds.Any())
            {
                filterPredicate = filterPredicate.And(p => req.ApplicationIds.Contains(p.ApplicationId.Value));
            }

            if (req.Status.Any())
            {
                filterPredicate = filterPredicate.And(p => req.Status.Contains(p.Status));
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
            var baseServiceResponse = new ServiceResponse<Package>();
#if NET452
            // Make the query
            if (filterPredicate.Parameters.Count > 0)
            {
                baseServiceResponse = packageServiceBase.GetIncluding(filterPredicate, includes.ToArray());
            }
            else
            {
                baseServiceResponse = packageServiceBase.GetAllIncluding(includes.ToArray());
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
                response.Code = ((short)PackageServiceResponseCodes.General_Exception).ToString();
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
                Body = "Packages successfuly fetched."
            });

            // Stop the sw
            sw.Stop();

            response.Type = ServiceResponseTypes.Success;
            response.Code = ((short)PackageServiceResponseCodes.Request_Successfuly_Completed).ToString();
            response.ServiceTook = sw.ElapsedMilliseconds;
            response.Message = "Packages successfuly fetched";
            response.LogRecords = logRecords;

            #endregion

            return response;
        }

        #endregion

    }
}
