using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CinemaSystem.WebApi.Helpers
{
    public class ErrorsFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ErrorsFilter> logger;

        public ErrorsFilter(ILogger<ErrorsFilter> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            this.logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
