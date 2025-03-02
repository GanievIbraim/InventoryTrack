using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InventoryTrack.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<InventoryStatus> InventoryStatus { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var baseEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType))
                .ToList();


            foreach (var entityType in baseEntityTypes)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("CreatedAt")
                    .HasDefaultValueSql("now()");

                modelBuilder.Entity(entityType.ClrType)
                    .Property("UpdatedAt")
                    .HasDefaultValueSql("now()");
            }
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var entries = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified)
                    .ToList();

                foreach (var entry in entries)
                {
                    if (entry.Entity is BaseEntity entity)
                    {
                        entity.UpdatedAt = DateTime.UtcNow;
                    }
                }
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving changes.", ex);
            }
        }




        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                   .Where(e => e.State == EntityState.Modified)
                   .ToList();

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }

}
