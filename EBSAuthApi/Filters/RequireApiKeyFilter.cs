using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace EBSAuthApi.Filters
{
    public class RequireApiKeyFilter : IAsyncActionFilter
    {
        private readonly string API_KEY_HEADER_NAME = "X-API-KEY";
        private readonly string API_KEY = "Auth:ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out StringValues extractedApiKey))
                throw new UnauthorizedAccessException("No api key provided");

            IConfiguration config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            string apiKey = config.GetValue<string>(API_KEY);

            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            if (!apiKey.Equals(extractedApiKey))
                throw new UnauthorizedAccessException("Api keys does not match");

            await next();
        }
    }
}

