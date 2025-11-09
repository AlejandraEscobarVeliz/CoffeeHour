using CoffeHour.Infrastructure.DTOs;

namespace CoffeeHour.Api.Responses
{
    /// <summary>
    /// Respuesta estándar de la API.
    /// </summary>
    public class ApiResponse<T>
    {
        private IEnumerable<DetallePedidoDTO> enumerable;

        public T Data { get; set; }
        public Pagination Pagination { get; set; }
        public Message[] Messages { get; set; }
        public object Reporte { get; }

        public ApiResponse(T data, bool v)
        {
            Data = data;
            Messages = new Message[] { new Message { Type = "Info", Description = "Request successful" } };
        }

        public ApiResponse(IEnumerable<DetallePedidoDTO> enumerable)
        {
            this.enumerable = enumerable;
        }

        public ApiResponse(object reporte)
        {
            Reporte = reporte;
        }
    }

    public class Pagination
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    public class Message
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
}




/*public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public ApiResponse(string v, bool v1) { }

        public ApiResponse(T? data, string message = "Operación exitosa", bool success = true)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> Fail(string message) => new(default, message, false);
    }*/