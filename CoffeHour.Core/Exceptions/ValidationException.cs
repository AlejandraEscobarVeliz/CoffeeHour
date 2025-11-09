namespace CoffeHour.Core.Exceptions
{
    /// <summary>
    /// Excepción para errores de validación.
    /// </summary>
    public class ValidationException : BusinessException
    {
        public List<string> Errors { get; set; } = new();

        public ValidationException(string message)
            : base(message, "VALIDATION_ERROR", 400)
        {
        }

        public ValidationException(string message, List<string> errors)
            : base(message, "VALIDATION_ERROR", 400)
        {
            Errors = errors;
        }

        public ValidationException(List<string> errors)
            : base("Errores de validación.", "VALIDATION_ERROR", 400)
        {
            Errors = errors;
        }
    }
}