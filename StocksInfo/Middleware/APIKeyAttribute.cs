//Created by Alexander Fields
using APIMiddleware.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace APIMiddleware.Filters
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Method)]
    public class APIKeyAttribute : System.Attribute, IAsyncActionFilter
    {
        private const string apiKeyHeaderName = "apikey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Microsoft.Extensions.Primitives.StringValues potentialKey = "";
            string clientIP = APIUtility.GetClientPublicIPAddress(context);

            if (!context.HttpContext.Request.Headers.TryGetValue(apiKeyHeaderName, out potentialKey))
            {
                System.Console.WriteLine($"API Key or correct API Key Name was not found from {clientIP} !");
                context.Result = new UnauthorizedResult();
                return;
            }

            IConfiguration configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            string apiKey = GetKey();

            if (!apiKey.Equals(potentialKey))
            {
                System.Console.WriteLine($"{clientIP} Provided the wrong key!");
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }

        internal string GetKey()
        {
            DotNetEnv.Env.Load();
            string encodedKey = System.Environment.GetEnvironmentVariable("APIKEY");
            return encodedKey;
        }
    }
}