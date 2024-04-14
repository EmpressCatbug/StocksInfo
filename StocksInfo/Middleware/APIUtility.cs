//Created by Alexander Fields

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace APIMiddleware.Utility
{
    public class APIUtility
    {
        public static string GetClientPublicIPAddress(HttpContext context)
        {
            try
            {
                string ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = context.Connection.RemoteIpAddress.ToString();
                }

                return ipAddress;
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($" There was a problem getting the IP Address from {context.ToString()}. It was assigned it an arbitrary 10.0.0.1 {ex.ToString()}");
                return "10.0.0.1";
            }
        }

        public static string GetClientPublicIPAddress(ActionExecutingContext context)
        {
            string ipAddress = string.Empty;

            // Check if the X-Forwarded-For header has any value
            if (context.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            // If X-Forwarded-For is empty, use the remote IP address
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            }

            return ipAddress;
        }
    }
}