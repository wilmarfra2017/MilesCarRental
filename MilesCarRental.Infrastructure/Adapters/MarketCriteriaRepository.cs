using Microsoft.EntityFrameworkCore;
using MilesCarRental.Domain.Ports;
using MilesCarRental.Infrastructure.DataSource;

namespace MilesCarRental.Infrastructure.Adapters;

/// <summary>
/// Repositorio para acceder a los criterios de mercado dentro de la infraestructura de MilesCarRental.
/// Provee funcionalidades para consultar los criterios de mercado basados en su identificador.
/// </summary>
[Repository]
public class MarketCriteriaRepository : IMarketCriteriaRepository
{
    private readonly DataContext _context;
    public MarketCriteriaRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Obtiene los criterios de mercado por su identificador único de forma asíncrona.
    public async Task<string> GetCriteriaByIdAsync(Guid marketCriteriaId)
    {
        var marketCriteria = await _context.MarketCriterias
            .AsNoTracking()
            .FirstOrDefaultAsync(mc => mc.Id == marketCriteriaId);

        return marketCriteria?.Criteria ?? string.Empty;
    }
}
