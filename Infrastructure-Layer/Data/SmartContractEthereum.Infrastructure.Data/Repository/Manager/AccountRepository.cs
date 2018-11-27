using SmartContractEthereum.Domain.Entities.Manager;
using SmartContractEthereum.Domain.Interfaces.Repositories.Manager;

namespace SmartContractEthereum.Infrastructure.Data.Repository.Manager
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
    }
}
