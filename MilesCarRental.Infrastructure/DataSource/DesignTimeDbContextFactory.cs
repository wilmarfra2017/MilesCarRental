using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MilesCarRental.Infrastructure.DataSource
{
    /// <summary>
    /// Fábrica que se utiliza en tiempo de diseño para la creación de instancias de DataContext"/>.
    /// Esto es especialmente útil para realizar migraciones de Entity Framework Core y otras operaciones
    /// en tiempo de diseño que requieren acceso a la base de datos.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        /// Crea una instancia de DataContext configurada con la cadena de conexión
        /// especificada en el archivo de configuración de la aplicación.
        public DataContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "MilesCarRental.Api"))
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("db");

            builder.UseSqlServer(connectionString);

            return new DataContext(builder.Options);
        }
    }
}
