using BuildingBlocks.Application.Abstract.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OrderService.Application.Queries.GetOrderItemsQuery;
using OrderService.Application.Queries.GetOrderQuery;
using System.Reflection;

namespace OrderService.Api.Endpoints
{
    public static partial class Endpoints
    {
        public static void MapQueries(IEndpointRouteBuilder app)
        {
            app.MapGet("/get-order", InvokeQueryAsync<GetOrderQuery, GetOrderQueryResponse>())
                .WithTags(OpenApiTag);

            app.MapGet("/get-order-items", InvokeQueryAsync<GetOrderItemsQuery, GetOrderItemsQueryResponse>())
                .WithTags(OpenApiTag);

            return;

            Delegate InvokeQueryAsync<TQuery, TQueryResponse>()
                where TQuery : IQuery<TQueryResponse>, new()
                where TQueryResponse : IQueryResponse
            {
                return ([FromServices] IMediator mediator,
                    [FromServices] IHttpContextAccessor httpContextAccessor,
                    CancellationToken cancellationToken) =>
                {
                    var query = new TQuery();
                    PropertyInfo[] properties = query.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo? property in properties)
                    {
                        if (httpContextAccessor.HttpContext!.Request.Query.TryGetValue(property.Name, out StringValues queryValue))
                        {
                            SetValue(httpContextAccessor, ref query, property, queryValue);
                        }

                        if (httpContextAccessor.HttpContext!.Request.RouteValues.TryGetValue(property.Name, out object? routeValue))
                        {
                            SetValue(httpContextAccessor, ref query, property, routeValue);
                        }
                    }

                    return mediator.Send(query, cancellationToken);
                };
            }
        }

        private static void SetValue<TQuery>(IHttpContextAccessor httpContextAccessor, ref TQuery query, PropertyInfo property, object? value)
            where TQuery : new()
        {
            if (value is null) return;

            if (property.PropertyType.IsAssignableFrom(typeof(bool)))
            {
                property.SetValue(query, bool.Parse(value.ToString()!));
            }
            else if (property.PropertyType.IsAssignableFrom(typeof(int)))
            {
                property.SetValue(query, int.Parse(value.ToString()!));
            }
            else if (property.PropertyType.IsAssignableFrom(typeof(long)))
            {
                property.SetValue(query, long.Parse(value.ToString()!));
            }
            else if (property.PropertyType.IsAssignableFrom(typeof(string)))
            {
                property.SetValue(query, value.ToString());
            }
            else if (property.PropertyType.IsAssignableFrom(typeof(string)))
            {
                property.SetValue(query, value.ToString());
            }
            else if (property.PropertyType.IsAssignableFrom(typeof(Guid)))
            {
                property.SetValue(query, Guid.Parse(value.ToString()!));
            }
            else
            {
                throw new NotSupportedException($"Type {property.PropertyType.Name} is not supported for binding");
            }
        }
    }
}
