using SmartContractEthereum.Domain.Entities.Manager;

namespace SmartContractEthereum.Domain.Entities
{
    public class EthereumContract : Entity
    {
        public string ContractID { get; set; }

        public virtual Account Account { get; set; }
    }
}
