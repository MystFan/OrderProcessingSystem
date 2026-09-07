using Microsoft.OpenApi;
using OrderService.Domain.Exceptions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrderService.Api.Infrastructure
{
    public class ExceptionReasonCodeDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            context.SchemaGenerator.GenerateSchema(typeof(ExceptionReasonCode), context.SchemaRepository);
        }
    }
}
