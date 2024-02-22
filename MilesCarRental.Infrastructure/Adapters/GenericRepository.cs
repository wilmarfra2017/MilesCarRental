using Microsoft.EntityFrameworkCore;
using MilesCarRental.Domain.Entities;
using MilesCarRental.Infrastructure.DataSource;
using MilesCarRental.Infrastructure.Ports;
using System.Linq.Expressions;

namespace MilesCarRental.Infrastructure.Adapters;

/// <summary>
/// Proporciona una implementación genérica del patrón de repositorio para entidades del dominio,
/// permitiendo realizar operaciones CRUD básicas sobre una base de datos utilizando Entity Framework.
/// </summary>
public class GenericRepository<T> : IRepository<T> where T : DomainEntity
{

    private readonly DbSet<T> _dataset;

    public GenericRepository(DataContext context)
    {
        var localContext = context ?? throw new ArgumentNullException(nameof(context));
        _dataset = localContext.Set<T>();
    }

    public async Task<T?> GetOneAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Invalid ID", nameof(id));
        }

        return await _dataset.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetManyAsync()
    {
        return await _dataset.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter)
    {
        return await _dataset.Where(filter).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
    {
        var query = _dataset.Where(filter);
        return await orderBy(query).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeStringProperties)
    {
        var query = _dataset.Where(filter).Include(includeStringProperties);
        return await orderBy(query).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeStringProperties, bool isTracking)
    {
        var query = isTracking ? _dataset.Where(filter).Include(includeStringProperties) : _dataset.AsNoTracking().Where(filter).Include(includeStringProperties);
        return await orderBy(query).ToListAsync();
    }
}
