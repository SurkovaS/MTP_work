namespace MTP1.Helpers
{
    using System;
    using System.Linq;
    using System.Linq.Dynamic;

    /// <summary>
    /// вспомогательные методы для пейджинга
    /// </summary>
    public static class Paging
    {
        public static int TotalPages(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling(totalCount / (float)pageSize);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> collection, string sortBy, int startRow, int maxRow)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                sortBy = "Id";
            }

            return collection.OrderBy(sortBy).Skip(startRow).Take(maxRow);
        }
    }
}