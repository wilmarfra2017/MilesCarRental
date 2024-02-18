using Microsoft.EntityFrameworkCore;
using MilesCarRental.Domain.Entities;

namespace MilesCarRental.Infrastructure.DataSource
{
    public class DataContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MarketCriteria> MarketCriterias { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            // Registra las entidades con el modelo
            modelBuilder.Entity<Vehicle>();
            modelBuilder.Entity<Location>();
            modelBuilder.Entity<Availability>();
            modelBuilder.Entity<Booking>();
            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<MarketCriteria>(entity =>
            {
                entity.HasKey(e => e.Id);
            });


            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Brand).IsRequired().HasMaxLength(100);
                entity.Property(v => v.Model).IsRequired().HasMaxLength(100);
                entity.Property(v => v.Type).IsRequired().HasMaxLength(50);
                entity.Property(v => v.Year).IsRequired();
                entity.Property(v => v.PricePerDay).IsRequired().HasColumnType("decimal(18,2)");
            });

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var t = entityType.ClrType;
                if (typeof(DomainEntity).IsAssignableFrom(t))
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn");
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModifiedOn");
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
