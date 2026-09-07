using System.Reflection;

namespace OrderService.Api.Endpoints
{
    public static partial class Endpoints
    {
        private static string OpenApiTag
        {
            get => Assembly.GetExecutingAssembly().GetName().Name!;
        }

        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            MapCommands(app);
            MapQueries(app);
        }
    }
}
