// CoffeHour.Core/Exceptions/NotFoundException.cs
namespace CoffeHour.Core.Exceptions
{
    /// <summary>
    /// Excepción cuando no se encuentra un recurso.
    /// </summary>
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string message)
            : base(message, "NOT_FOUND", 404)
        {
        }

        public NotFoundException(string resourceName, object key)
            : base($"{resourceName} con ID '{key}' no fue encontrado.", "NOT_FOUND", 404)
        {
        }
    }
}