
using Microsoft.EntityFrameworkCore;
using CoffeHour.Infrastructure.Data;
using CoffeHour.Infrastructure.Mappings;
using CoffeHour.Infrastructure.Repositories;
using CoffeHour.Core.Interfaces;
using CoffeHour.Infrastructure.Validators;

using FluentValidation;
using CoffeHour.Infrastructure.Validators;
using FluentValidation.AspNetCore;


namespace CoffeHour.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // 🔹 CONFIGURAR BASE DE DATOS

            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<CoffeeHourContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // 🔹 AUTOMAPPER
           
            builder.Services.AddAutoMapper(typeof(MappingProfile));

      
            // 🔹 INYECCIÓN DE DEPENDENCIAS
          
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
            builder.Services.AddScoped<IDetallePedidoRepository, DetallePedidoRepository>();


            // ===========================
            // 🔹 VALIDADORES AUTOMÁTICOS
            // ===========================
            builder.Services.AddValidatorsFromAssemblyContaining<ClienteValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ProductoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<PedidoValidator>();




            // 🔹 VALIDACIÓN PERSONALIZADA

            builder.Services.AddScoped<IValidationService, ValidationService>();

            builder.Services
    .AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());



            // 🔹 CONTROLADORES

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Evita que ASP.NET devuelva errores automáticos de validación
                    options.SuppressModelStateInvalidFilter = true;
                });

            var app = builder.Build();

            // 🔹 PIPELINE HTTP
          
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
