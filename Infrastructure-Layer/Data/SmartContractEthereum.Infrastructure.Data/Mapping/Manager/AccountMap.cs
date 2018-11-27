using SmartContractEthereum.Domain.Entities.Manager;
using System.Data.Entity.ModelConfiguration;

namespace SmartContractEthereum.Infrastructure.Data.Mapping.Manager
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            Property(t => t.Login)
              .IsRequired()
              .HasMaxLength(25);

            Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Account");
        }
    }
}