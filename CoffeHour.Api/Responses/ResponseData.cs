namespace CoffeeHour.Api.Responses
{
    /// <summary>
    /// Wrapper para respuestas que incluyen datos y metadata de paginación.
    /// </summary>
    public class ResponseData<T>
    {
        public T? Data { get; set; }
        public object? Pagination { get; set; } // puedes usar Pagination tipo-safe

        public ResponseData() { }

        public ResponseData(T data, object? pagination = null)
        {
            Data = data;
            Pagination = pagination;
        }
    }
}

