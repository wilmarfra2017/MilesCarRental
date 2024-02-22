using Microsoft.EntityFrameworkCore;
using MilesCarRental.Domain.Ports;
using MilesCarRental.Infrastructure.DataSource;

namespace MilesCarRental.Infrastructure.Adapters;

/// <summary>
/// Implementa el patrón Unit of Work para la aplicación MilesCarRental, encapsulando el contexto
/// de la base de datos y centralizando la lógica para guardar los cambios realizados en las entidades
/// durante el ciclo de vida de una transacción de negocio.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    //Método interno que guarda los cambios en el contexto de la base de datos, aplicando automáticamente
    //las marcas de tiempo para las propiedades de auditoría como 'CreatedOn' y 'LastModifiedOn'.
    private async Task InternalSaveAsync(CancellationToken cancellationToken)
    {
        _context.ChangeTracker.DetectChanges();
        var entryStatus = new Dictionary<EntityState, string>
            {
                { EntityState.Added, "CreatedOn" },
                { EntityState.Modified, "LastModifiedOn" }
            };

        foreach (var entry in _context.ChangeTracker.Entries())
        {
            if (entryStatus.TryGetValue(entry.State, out var propertyName))
            {
                entry.Property(propertyName).CurrentValue = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    // Guarda todos los cambios realizados en el contexto de la base de datos de forma asíncrona.
    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await InternalSaveAsync(cancellationToken);
    }

    // Guarda todos los cambios realizados en el contexto de la base de datos de forma asíncrona sin proporcionar un token de cancelación.
    public async Task SaveAsync()
    {
        await InternalSaveAsync(CancellationToken.None);
    }
}