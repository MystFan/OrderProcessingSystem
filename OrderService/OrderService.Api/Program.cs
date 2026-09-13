using BuildingBlocks.Application.Abstract;
using BuildingBlocks.Application.Behaviors;
using BuildingBlocks.DataAccess;
using BuildingBlocks.Infrastructure;
using FluentValidation;
using MassTransit;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OrderService.Api.Endpoints;
using OrderService.Api.Infrastructure;
using OrderService.Application;
using OrderService.DataAccess;
using OrderService.Domain.Exceptions;
using Scalar.AspNetCore;
using System.Reflection.Metadata;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace OrderService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.Configure<RouteHandlerOptions>(options =>
            {
                // By default no BadHttpRequestException exception is throw in non-development environment.
                // Configure options to throw the exception in all environments and handle it into the exception handler middleware and write a correct response.
                options.ThrowOnBadRequest = true;
            });

            builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.SectionName));
            builder.Services.Configure<RabbitMQOptions>(builder.Configuration.GetSection(RabbitMQOptions.SectionName));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHealthChecks();
            builder.Services.AddExceptionHandler<ExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddDbContext<OrderServiceEFContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork<OrderServiceEFContext>>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(OrderServiceRepository<>));
            builder.Services.AddSingleton(TimeProvider.System);

            builder.Services.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(IApplicationAssemblyMarker).Assembly);
                cfg.AddOpenBehavior(typeof(RequestPreProcessorBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(CommitBehavior<,>));
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(IApplicationAssemblyMarker).Assembly);
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(builder.Configuration["RABBITMQ:HOST"], "/", hostConfig =>
                    {
                        hostConfig.Username(builder.Configuration["RABBITMQ:USERNAME"]!);
                        hostConfig.Password(builder.Configuration["RABBITMQ:PASSWORD"]!);
                    });

                    config.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
            builder.Services.AddHostedService<OutboxPublisherHostedService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi("v1", options =>
            {
                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = "Order Service API",
                        Version = "1.0",
                        Description = "Order Service API",
                    };

                    if (!document.Components!.Schemas!.ContainsKey(nameof(ExceptionReasonCode)))
                    {
                        var enumSchema = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String,
                            Enum = Enum.GetNames(typeof(ExceptionReasonCode))
                                .Select(name => JsonValue.Create(name))
                                .Cast<JsonNode>()
                                .ToList()
                        };

                        document.Components.Schemas.Add(nameof(ExceptionReasonCode), enumSchema);
                    }

                    return Task.CompletedTask;
                });
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi("/openapi/v1.json");
                app.MapScalarApiReference();
            }

            app.UseHealthChecks("/health/ping", new HealthCheckOptions
            {
                ResponseWriter = (ctx, _) => ctx.Response.WriteAsync("pong")
            });

            app.UseExceptionHandler();
            app.MapEndpoints();

            app.Run();
        }
    }
}
