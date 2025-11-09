
// CoffeHour.Infrastructure/Filters/GlobalExceptionFilter.cs
using CoffeHour.Core.Exceptions; // ✅ ASEGURAR QUE ESTÉ ESTE USING
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CoffeHour.Infrastructure.Filters
{
    /// <summary>
    /// Filtro global para manejar excepciones de forma centralizada.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var statusCode = GetStatusCode(exception);
            var errorResponse = CreateErrorResponse(exception, statusCode);

            LogException(exception, statusCode);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = statusCode
            };
            context.ExceptionHandled = true;
        }

        private int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                NotFoundException => 404, // ✅ Ahora reconocerá esta excepción
                ValidationException => 400,
                BusinessException businessEx => businessEx.StatusCode,
                UnauthorizedAccessException => 401,
                ArgumentException => 400,
                KeyNotFoundException => 404,
                NotImplementedException => 501,
                _ => 500
            };
        }

        private object CreateErrorResponse(Exception exception, int statusCode)
        {
            var baseResponse = new
            {
                Type = exception.GetType().Name,
                Message = exception.Message,
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow
            };

            // ✅ Si es ValidationException, incluir errores
            if (exception is ValidationException validationEx)
            {
                return new
                {
                    baseResponse.Type,
                    baseResponse.Message,
                    baseResponse.StatusCode,
                    baseResponse.Timestamp,
                    ErrorCode = "VALIDATION_ERROR",
                    Errors = validationEx.Errors
                };
            }

            // Si es BusinessException, incluir ErrorCode
            if (exception is BusinessException businessEx)
            {
                return new
                {
                    baseResponse.Type,
                    baseResponse.Message,
                    baseResponse.StatusCode,
                    baseResponse.Timestamp,
                    ErrorCode = businessEx.ErrorCode
                };
            }

            return baseResponse;
        }

        private void LogException(Exception exception, int statusCode)
        {
            var logMessage = $"[{statusCode}] {exception.GetType().Name}: {exception.Message}";

            if (statusCode >= 500)
                _logger.LogError(exception, logMessage);
            else if (statusCode >= 400)
                _logger.LogWarning(exception, logMessage);
            else
                _logger.LogInformation(logMessage);
        }
    }
}