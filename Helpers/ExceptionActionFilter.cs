using System;
using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace WeCareWebApp.Helpers
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly TelemetryClient _telemetryClient;

        public ExceptionActionFilter(
        IWebHostEnvironment hostingEnvironment,
        TelemetryClient telemetryClient)
        {
            _hostingEnvironment = hostingEnvironment;
            _telemetryClient = telemetryClient;
        }

        #region Overrides of ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            var actionDescriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;
            Type controllerType = actionDescriptor.ControllerTypeInfo;

            var controllerBase = typeof(ControllerBase);
            var controller = typeof(Controller);

            if (controllerType.IsSubclassOf(controllerBase) && !controllerType.IsSubclassOf(controller))
            {

            }

            if (controllerType.IsSubclassOf(controllerBase) && controllerType.IsSubclassOf(controller))
            {

            }
            if (!_hostingEnvironment.IsDevelopment())
            {
                _telemetryClient.TrackException(context.Exception);
                _telemetryClient.Flush();
            }
            base.OnException(context);
        }
        #endregion
    }
}
