using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Services.ExtensionsServices
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParameters<T>(this HttpContext httpContext,
            IQueryable<T> queryable, int registersPerPageQuantity)
        {
            double count = await queryable.CountAsync();
            double pagesQuantity = Math.Ceiling(count / registersPerPageQuantity);
            httpContext.Response.Headers.Add("PagesQuantity", pagesQuantity.ToString());
        }
    }
}
