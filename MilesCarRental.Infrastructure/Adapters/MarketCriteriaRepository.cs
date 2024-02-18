using Microsoft.EntityFrameworkCore;
using MilesCarRental.Domain.Ports;
using MilesCarRental.Infrastructure.DataSource;

namespace MilesCarRental.Infrastructure.Adapters;

[Repository]
public class MarketCriteriaRepository : IMarketCriteriaRepository
{
    private readonly DataContext _context;
    public MarketCriteriaRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<string> GetCriteriaByIdAsync(Guid marketCriteriaId)
    {
        var marketCriteria = await _context.MarketCriterias
            .AsNoTracking()
            .FirstOrDefaultAsync(mc => mc.Id == marketCriteriaId);

        return marketCriteria?.Criteria ?? string.Empty;
    }
}
