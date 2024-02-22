using MilesCarRental.Domain.Entities;
using System.Linq.Expressions;

namespace MilesCarRental.Infrastructure.Ports
{
    /// <summary>
    /// Define un contrato genérico para un repositorio que encapsula el conjunto de operaciones de lectura
    /// comunes para trabajar con entidades del dominio. Esta interfaz asegura que las implementaciones
    /// proporcionen métodos para obtener una o varias entidades basadas en varios criterios.
    /// </summary>
    public interface IRepository<T> where T : DomainEntity
    {
        Task<T?> GetOneAsync(Guid id);

        Task<IEnumerable<T>> GetManyAsync();
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeStringProperties);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeStringProperties, bool isTracking);
    }
}
