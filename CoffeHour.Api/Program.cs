using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Data;
using CoffeHour.Infrastructure.Filters;
using CoffeHour.Infrastructure.Mappings;
using CoffeHour.Infrastructure.Repositories;
using CoffeHour.Infrastructure.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace CoffeHour.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ----------- CONFIGURAR BASE DE DATOS -----------
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<CoffeeHourContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // ----------- AUTO MAPPER -----------
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // ----------- REPOSITORIOS Y UNIT OF WORK -----------
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
            builder.Services.AddScoped<IDetallePedidoRepository, DetallePedidoRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ----------- DAPPER -----------
            builder.Services.AddScoped<DapperContext>();

            // ----------- VALIDACIONES -----------
            builder.Services.AddValidatorsFromAssemblyContaining<ClienteValidator>();

            // ----------- CONTROLADORES + FILTROS -----------
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // ----------- SWAGGER -----------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CoffeeHour API",
                    Version = "v1",
                    Description = "API REST para la gestión de la cafetería CoffeeHour ☕"
                });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            // ----------- CONSTRUIR APLICACIÓN -----------
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Cafetería CoffeeHour v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
