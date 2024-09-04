using EnviBad.API.Common;
using Microsoft.OpenApi.Models;

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

            var app = builder.Build();

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
