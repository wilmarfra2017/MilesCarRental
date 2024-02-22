using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MilesCarRental.Infrastructure.DataSource;

namespace MilesCarRental.Api.Tests
{
    /// <summary>
    /// Clase de configuración para el entorno de pruebas de la API de MilesCarRental.
    /// se utiliza para configurar el entorno de pruebas
    /// con una base de datos en memoria, permitiendo la ejecución de pruebas de integración    
    /// </summary>
    public class MilesCarRentalApiApp : WebApplicationFactory<Program>
    {        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Personaliza los servicios para el entorno de pruebas
            builder.ConfigureServices(services =>
            {
                // Busca y elimina la configuración existente de DbContextOptions para DataContext,
                // para reemplazarla con una configuración que utiliza una base de datos en memoria.
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Agrega y configura el DataContext para usar una base de datos en memoria,
                // aislando así las pruebas de integración de la base de datos real.
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryMilesCarRenta");
                });

            });
        }
    }
}
