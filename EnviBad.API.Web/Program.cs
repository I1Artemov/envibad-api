using EnviBad.API.Common;
using EnviBad.API.Common.Log;
using EnviBad.API.Common.Models.Options;
using EnviBad.API.Infrastructure.Contexts;
using EnviBad.API.Infrastructure.Interfaces;
using EnviBad.API.Infrastructure.Log;
using EnviBad.API.Infrastructure.MessageQueue;
using EnviBad.API.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using Npgsql;

namespace EnviBad.API.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = Const.AppVersion,
                    Title = "EnviBad Reports API",
                    Description = "An ASP.NET Core Web API запроса отчетов EnviBad",
                    Contact = new OpenApiContact
                    {
                        Name = "Автор сервиса",
                        Url = new Uri("https://t.me/CrypticPassage")
                    }
                });
                string filePath = Path.Combine(System.AppContext.BaseDirectory, "EnviBad.API.Web.xml");
                options.IncludeXmlComments(filePath);
            });

            IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
            string connectionStr = builder.Configuration.GetConnectionString("EnviBadPostgres");
            var dsBuilder = new NpgsqlDataSourceBuilder(connectionStr);
            var dbDataSource = dsBuilder.Build();
            builder.Services.AddDbContext<EnviBadApiContext>(options =>
            {
                options.UseNpgsql(dbDataSource);
            });

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    MassTransitOptions? rabbitSettings = builder.Configuration
                        .GetSection("MassTransitOptions")
                        .Get<MassTransitOptions>();

                    cfg.Host(rabbitSettings?.RabbitHost, rabbitSettings?.RabbitPort ?? 5672, "/envibad", h =>
                    {
                        h.Username(rabbitSettings?.RabbitUser ?? "guest");
                        h.Password(rabbitSettings?.RabbitPassword ?? "guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddScoped<IUserReportRequestRepo, UserReportRequestRepo>();
            builder.Services.AddScoped<IMassTransitPublisher, MassTransitPublisher>();

            var logger = LogManager.GetLogger("AppLogger");
            builder.Services.AddSingleton<NLog.ILogger>(logger);
            builder.Services.AddScoped<IAppLogger, AppLoggerImpl>();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            // app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
