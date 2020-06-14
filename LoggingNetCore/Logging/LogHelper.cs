using Microsoft.AspNetCore.Http;
using Serilog;

namespace LoggingNetCore.Logging
{
    public static class LogHelper
    {
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            diagnosticContext.Set("UserName", httpContext.User.Identity.Name);

            // Retrieve the IEndpointFeature selected for the request
            var endpoint = httpContext.GetEndpoint();
            diagnosticContext.Set("EndpointName", endpoint?.DisplayName);
        }
    }
}
