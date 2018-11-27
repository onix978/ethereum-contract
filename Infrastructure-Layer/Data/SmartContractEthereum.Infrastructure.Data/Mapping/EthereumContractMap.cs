using SmartContractEthereum.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace SmartContractEthereum.Infrastructure.Data.Mapping
{
    public class EthereumContractMap : EntityTypeConfiguration<EthereumContract>
    {
        public EthereumContractMap()
        {
            // Primary Key
            HasKey(t => t.ID);

            // Properties
            Property(t => t.ContractID)
                .HasMaxLength(150);

            // Table & Column Mappings
            ToTable("EthereumContract");
            Property(t => t.ID).HasColumnName("ID");
            Property(t => t.ContractID).HasColumnName("ContractID");
        }
    }
}
