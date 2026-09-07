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
using System.Text.Json.Serialization;

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
            builder.Services.AddSwaggerGen(options =>
            {
                options.DocumentFilter<ExceptionReasonCodeDocumentFilter>();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OrderService.Api"
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
                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("v1/swagger.json", "Server API Schema v1"));
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
