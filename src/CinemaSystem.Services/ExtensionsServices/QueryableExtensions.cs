using CinemaSystem.Models.DTOs;
using System.Linq;

namespace CinemaSystem.Services.ExtensionsServices
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            IQueryable<T> _queryable = queryable
                .Skip((paginationDto.Page - 1) * paginationDto.RegistersPerPageQuantity)
                .Take(paginationDto.RegistersPerPageQuantity);

            return _queryable;
        }
    }
}
