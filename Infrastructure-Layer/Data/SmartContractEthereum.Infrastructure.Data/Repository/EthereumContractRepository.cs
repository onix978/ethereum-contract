using SmartContractEthereum.Domain.Entities;
using SmartContractEthereum.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SmartContractEthereum.Infrastructure.Data.Repository
{
    public class EthereumContractRepository : Repository<EthereumContract>, IEthereumContractRepository
    {
    }
}
