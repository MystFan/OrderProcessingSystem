using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.Exceptions;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderService.Api.Infrastructure
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is OrderServiceDomainException)
            {
                if (exception.InnerException != null)
                {
                    _logger.LogError(exception, exception.Message);
                }

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                await WriteResponseAsync(context, exception);
            }
            else
            {
                _logger.LogError(exception, exception.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                await WriteResponseAsync(context, exception);
            }

            return true;
        }

        private static Task WriteResponseAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "An error occured while processing the request.",
                Extensions =
                {
                    {
                        "message", exception.ToString()
                    }
                }
            };

            if (exception is OrderServiceDomainException domainServicesException)
            {
                if (domainServicesException.AdditionalData.ValidationErrors != null)
                {
                    domainServicesException.AdditionalData.ValidationErrors = domainServicesException.AdditionalData.ValidationErrors
                        .Select(error => new
                        {
                            key = error.Key[..1].ToLowerInvariant() + error.Key[1..],
                            value = error.Value
                        }).ToDictionary(error => error.key, error => error.value);
                }

                problemDetails.Extensions["message"] = exception.Message;
                problemDetails.Extensions.Add("additionalData", domainServicesException.AdditionalData);
                problemDetails.Extensions.Add("code", domainServicesException.ReasonCode.ToString());
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }
}
