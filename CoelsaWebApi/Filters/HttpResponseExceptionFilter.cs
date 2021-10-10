using Microsoft.AspNetCore.Mvc.Filters;
using CoelsaCommon.Models;
using Microsoft.AspNetCore.Mvc;
using CoelsaCommon.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoelsaWebApi.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger<HttpResponseExceptionFilter> _logger;
        public HttpResponseExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpResponseExceptionFilter>();
        }
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        /// <summary>
        /// This method is executed after an exception was thrown
        /// </summary>
        /// <param name="context">A context for action filters</param>
        public void OnActionExecuted(ActionExecutedContext context) 
        { 
            if(context.Exception == null)
            {
                return;
            }

            if (context.Exception is HttpResponseException exception)
            {
                context.Result = new ObjectResult(exception.Value)
                {
                    StatusCode = exception.Status,
                };

                context.ExceptionHandled = true;

                _logger.LogError($"Exception of type {nameof(HttpResponseException)} has been caught in {context.ActionDescriptor.DisplayName}");
            }
            else if (context.Exception is ValidationException validationException)
            {
                context.Result = new ObjectResult(
                new { validationException.Message, validationException.Field, validationException.ExtraData })
                {
                    StatusCode = 400,
                };

                context.ExceptionHandled = true;

                _logger.LogError($"Exception of Type {nameof(ValidationException)}, encountered in: {context.ActionDescriptor.DisplayName}, error: {validationException.Message}, data:{JsonConvert.SerializeObject(validationException.ExtraData)}");
            }
            else
            {
                context.Result = new ObjectResult(
                new { Message = context.Exception.Message })
                {
                    StatusCode = 500
                };

                context.ExceptionHandled = true;

                _logger.LogError($"Exception encountered in: {context.ActionDescriptor.DisplayName}, error: {context.Exception.Message}");
            }


        }
    }
}
