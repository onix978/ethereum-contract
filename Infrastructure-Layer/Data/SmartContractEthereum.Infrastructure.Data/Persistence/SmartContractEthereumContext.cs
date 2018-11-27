using SmartContractEthereum.Domain.Entities;
using SmartContractEthereum.Domain.Entities.Manager;
using SmartContractEthereum.Infrastructure.Data.Mapping;
using SmartContractEthereum.Infrastructure.Data.Mapping.Manager;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace SmartContractEthereum.Infrastructure.Data.Persistence
{
    public partial class SmartContractEthereumContext : DbContext
    {
        public SmartContractEthereumContext() : base("SmartContractEthereumContext")
        {
        }

        public DbSet<Account> Account { get; set; }
        public DbSet<EthereumContract> EthereumContract { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<SmartContractEthereumContext>(null);

            modelBuilder.Configurations.Add(new EthereumContractMap());
            modelBuilder.Configurations.Add(new AccountMap());

            modelBuilder.Properties<string>().Configure(x => x.HasColumnType("varchar"));
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Created") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("Created").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property("Created").IsModified = false;
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Active") != null))
            {
                if ((entry.State == EntityState.Added || entry.State == EntityState.Modified) && entry.Property("Active").CurrentValue == null)
                    entry.Property("Active").CurrentValue = true;
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Updated") != null))
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("Updated").CurrentValue = DateTime.Now;
                    entry.Property("Created").IsModified = true;
                }
            }

            try {  return base.SaveChanges(); }
            catch (DbEntityValidationException exception)
            {
                Exception raise = exception;

                foreach (var validationErrors in exception.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);
                     
                        raise = new InvalidOperationException(message, raise);
                    }

                throw raise;
            }
            catch (DbUpdateException exception)
            {
                Exception raise = exception;
                throw raise;
            }

            catch (Exception exception)
            {
                Exception raise = exception;
                throw raise;
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Created") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("Created").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property("Created").IsModified = false;
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Active") != null))
            {
                if ((entry.State == EntityState.Added || entry.State == EntityState.Modified) && entry.Property("Active").CurrentValue == null)
                    entry.Property("Active").CurrentValue = true;
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Updated") != null))
            {
                if (entry.State == EntityState.Modified)
                    entry.Property("Updated").CurrentValue = DateTime.Now;
            }

            try { return await base.SaveChangesAsync(); }
            catch (DbEntityValidationException exception)
            {
                Exception raise = exception;

                foreach (var validationErrors in exception.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}", validationErrors.Entry.Entity.ToString(), validationError.ErrorMessage);

                        raise = new InvalidOperationException(message, raise);
                    }

                throw raise;
            }
            catch (DbUpdateException exception)
            {
                Exception raise = exception;
                throw raise;
            }

            catch (Exception exception)
            {
                Exception raise = exception;
                throw raise;
            }
        }
    }
}
