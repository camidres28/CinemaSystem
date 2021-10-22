using CinemaSystem.Services.MovieServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace CinemaSystem.WebApi.CustomFilters
{
    public class ExistsMovieAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly IMovieServices movieServices;

        public ExistsMovieAttribute(IMovieServices movieServices)
        {
            this.movieServices = movieServices;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, 
            ResourceExecutionDelegate next)
        {
            object movieIdObj = context.HttpContext.Request.RouteValues["movieId"];
            if (movieIdObj == null)
            {
                return;
            }

            int movieId = int.Parse(movieIdObj.ToString());
            bool exists = await this.movieServices.ExistsMovieAsync(movieId);
            if (!exists)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                await next();
            }
        }
    }
}
