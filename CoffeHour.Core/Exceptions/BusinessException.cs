
namespace CoffeHour.Core.Exceptions
{
    /// <summary>
    /// Excepción personalizada para reglas de negocio.
    /// </summary>
    public class BusinessException : Exception
    {
        public int StatusCode { get; set; } = 400;
        public string ErrorCode { get; set; } = "BUSINESS_ERROR";

        public BusinessException(string message, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }

        public BusinessException(string message, string errorCode, int statusCode = 400)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
