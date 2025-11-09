namespace CoffeeHour.Api.Responses
{
    /// <summary>
    /// Respuesta paginada con metadata.
    /// </summary>
    public class PagedResponse<T> : ApiResponse<IEnumerable<T>>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        public PagedResponse(IEnumerable<T> data, int page, int pageSize, int totalRecords)
            : base(data)
        {
            CurrentPage = page;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        }
    }
}

