using CoffeHour.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
//using CoffeHour.Api.Responses;

namespace CoffeHour.Infrastructure.Filters
{
    /// <summary>
    /// Filtro global para manejar excepciones no controladas y errores de negocio.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        //private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var statusCode = exception is BusinessException ? (exception as BusinessException).StatusCode : 500;
            var errorResponse = new { Message = exception.Message, StatusCode = statusCode };

            _logger.LogError(exception, "Global exception caught");

            context.Result = new ObjectResult(errorResponse) { StatusCode = statusCode };
            context.ExceptionHandled = true;
        }
    }
}

