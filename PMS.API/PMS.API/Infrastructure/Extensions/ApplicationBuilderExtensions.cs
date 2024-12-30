using Microsoft.AspNetCore.Diagnostics;
using PMS.Shared.Exceptions;
using System.Text.Json;

namespace PMS.API.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomErrorHandlingMiddleware(
          this IApplicationBuilder builder)
        {
            return builder.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;

                    var error = new ErrorResponse
                    {
                        Error = new Error
                        {
                            Message = exception.Message,
                            Details = new List<ErrorDetail>
                    {
                        new ErrorDetail
                        {
                            Target = exception.TargetSite.ToString(),
                            Message=exception.Message
                        }
                    }
                        }
                    };

                    if (exception.InnerException != null)
                    {
                        var innerException = exception.InnerException;
                        error.Error.Details.Add(new ErrorDetail
                        {
                            Target = innerException.TargetSite?.ToString(),
                            Message = innerException.Message
                        });
                    }
                    await context.Response.WriteAsJsonAsync(
                        error,
                        error.GetType(),
                        new JsonSerializerOptions(),
                        contentType: "application/problem+json"
                    );
                });
            });
        }
    }
}
