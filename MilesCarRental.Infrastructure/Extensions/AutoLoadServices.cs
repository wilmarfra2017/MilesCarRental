using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilesCarRental.Domain.Ports;
using MilesCarRental.Domain.Services;
using MilesCarRental.Infrastructure.Adapters;
using MilesCarRental.Infrastructure.Ports;

namespace MilesCarRental.Infrastructure.Extensions;

/// <summary>
/// Clase de extensión para IServiceCollection que facilita la carga automática y el registro de servicios y repositorios
/// mediante reflexión, simplificando la configuración de la inyección de dependencias.
/// </summary>
public static class AutoLoadServices
{
    // Extiende IServiceCollection para incluir la carga automática de servicios y repositorios basándose en atributos personalizados.
    public static IServiceCollection LoadServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IMarketCriteriaService, MarketCriteriaService>();
        services.AddSingleton(typeof(ILogMessageService), typeof(LogMessageService));

        var _services = AppDomain.CurrentDomain.GetAssemblies()
              .Where(assembly =>
              {
                  return (assembly.FullName is null) || assembly.FullName.Contains("Domain", StringComparison.InvariantCulture);
              })
              .SelectMany(s => s.GetTypes())
              .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));

        // Ditto, but repositories
        var _repositories = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly =>
            {
                return (assembly.FullName is null) || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture);
            })
            .SelectMany(s => s.GetTypes())
            .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(RepositoryAttribute)));

        // svc
        foreach (var service in _services)
        {
            services.AddTransient(service);
        }

        // repos
        foreach (var repo in _repositories)
        {
            Type iface = repo.GetInterfaces().Single();
            services.AddTransient(iface, repo);
        }

        return services;
    }

}
