using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CinemaAPI.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var result = new
            {
                Message = "Something went wrong",
                Error = context.Exception.Message
            };

            context.Result = new ObjectResult(result)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}
