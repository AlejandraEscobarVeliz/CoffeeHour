using Microsoft.EntityFrameworkCore;

namespace CoffeHour.Core.CustomEntities
{
    /// <summary>
    /// Representa una lista paginada con metadatos.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad a paginar.</typeparam>
    public class PagedList<T> : List<T>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;

        public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            AddRange(items);
            TotalCount = totalCount;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
        }

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }

}

