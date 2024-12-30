using Microsoft.AspNetCore.Http;
using System.Net;

namespace PMS.Shared.BaseRepository.Extensions
{
    public static class HttpContextExtensions
    {
        public static IPAddress GetIpAddress(this HttpContext httpContext)
        {
            IPAddress ipAddress = IPAddress.Parse(httpContext.Connection.RemoteIpAddress.ToString());
            try
            {
                ipAddress = IPAddress.Parse(httpContext.Request.Headers["X-Forwarded-For"].ToString());
            }
            catch (Exception e)
            {
                //
            }

            return ipAddress;
        }
    }
}