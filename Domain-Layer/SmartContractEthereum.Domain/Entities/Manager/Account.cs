using SmartContractEthereum.Domain.Utilities;
using System.Security.Cryptography;

namespace SmartContractEthereum.Domain.Entities.Manager
{
    public class Account : Entity
    {
        public string EthereumAddress { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public void EncryptPassword()
        {
            Password = Encrypt.GetMd5Hash(MD5.Create(), Password);
        }
    }
}
