using Microsoft.AspNetCore.Mvc.Filters;
using CoelsaCommon.Models;
using Microsoft.AspNetCore.Mvc;
using CoelsaCommon.Validation;

namespace CoelsaWebApi.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
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
            }
            else if (context.Exception is ValidationException validationException)
            {
                context.Result = new ObjectResult(
                new { validationException.Message, validationException.Field, validationException.ExtraData })
                {
                    StatusCode = 400,
                };

                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new ObjectResult(
                new { Message = context.Exception.Message })
                {
                    StatusCode = 500
                };

                context.ExceptionHandled = true;
            }


        }
    }
}
