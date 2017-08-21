using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Clouderrorreporting.v1beta1;
using Google.Apis.Clouderrorreporting.v1beta1.Data;
using Google.Apis.Services;
using static Google.Apis.Clouderrorreporting.v1beta1.ProjectsResource.EventsResource;
namespace ChannelAdvisorSOAP
{
    public abstract class AErrorChild
    {
        private static ClouderrorreportingService CreateErrorReportingClient()
        {
            // Get the Application Default Credentials.
            GoogleCredential credential = GoogleCredential.GetApplicationDefaultAsync().Result;

            // Add the needed scope to the credentials.
            credential.CreateScoped(ClouderrorreportingService.Scope.CloudPlatform);

            // Create the Error Reporting Service.
            ClouderrorreportingService service = new ClouderrorreportingService(new BaseClientService.Initializer
            {
                ApiKey = "",
            });
            return service;
        }

        /// <summary>
        /// Creates a <seealso cref="ReportRequest"/> from a given exception.
        /// </summary>
        private static ReportRequest CreateReportRequest(Exception e)
        {
            // Create the service.
            ClouderrorreportingService service = CreateErrorReportingClient();

            // Get the project ID from the environement variables.
            string projectId = "premium-trainer-167312";

            // Format the project id to the format Error Reporting expects. See:
            // https://cloud.google.com/error-reporting/reference/rest/v1beta1/projects.events/report
            string formattedProjectId = string.Format("projects/{0}", projectId);

            // Add a service context to the report.  For more details see:
            // https://cloud.google.com/error-reporting/reference/rest/v1beta1/projects.events#ServiceContext
            ServiceContext serviceContext = new ServiceContext()
            {
                Service = "Channel Advisor SOAP API",
                Version = System.Windows.Forms.Application.ProductVersion,
            };
            ReportedErrorEvent errorEvent = new ReportedErrorEvent()
            {
                Message = e.ToString(),
                ServiceContext = serviceContext,
            };
            return new ReportRequest(service, errorEvent, formattedProjectId);
        }
        /// <summary>
        /// Creates a <seealso cref="ReportRequest"/> from a given exception.
        /// </summary>
        private static ReportRequest CreateReportRequest(Exception e, string WebMethod, string TargetURL, int ResponseCode)
        {
            // Create the service.
            ClouderrorreportingService service = CreateErrorReportingClient();

            // Get the project ID from the environement variables.
            string projectId = "premium-trainer-167312";

            // Format the project id to the format Error Reporting expects. See:
            // https://cloud.google.com/error-reporting/reference/rest/v1beta1/projects.events/report
            string formattedProjectId = string.Format("projects/{0}", projectId);

            // Add a service context to the report.  For more details see:
            // https://cloud.google.com/error-reporting/reference/rest/v1beta1/projects.events#ServiceContext
            ServiceContext serviceContext = new ServiceContext()
            {
                Service = "Just In Time",
                Version = System.Windows.Forms.Application.ProductVersion
            };
            ReportedErrorEvent errorEvent = new ReportedErrorEvent()
            {
                Message = e.ToString(),
                Context = new ErrorContext()
                {
                    HttpRequest = new HttpRequestContext()
                    {
                        Method = WebMethod,
                        Url = TargetURL,
                        ResponseStatusCode = ResponseCode
                    }
                },
                ServiceContext = serviceContext,
            };
            return new ReportRequest(service, errorEvent, formattedProjectId);
        }

        /// <summary>
        /// Report an exception to the Error Reporting service.
        /// </summary>
        protected static void Report(Exception e)
        {
            // Create the report and execute the request.
            ReportRequest request = CreateReportRequest(e);
            request.Execute();
        }
        protected static void Report(Exception e, string WebMethod, string TargetURL, int ResponseCode)
        {
            // Create the report and execute the request.
            ReportRequest request = CreateReportRequest(e, WebMethod, TargetURL, ResponseCode);
            request.Execute();
        }
    }
}
