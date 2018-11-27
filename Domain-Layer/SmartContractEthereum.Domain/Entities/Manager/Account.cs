using SmartContractEthereum.Domain.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace SmartContractEthereum.Domain.Entities.Manager
{
    public class Account : Entity
    {
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        public string Password { get; set; }

        public void EncryptPassword()
        {
            Password = Encrypt.GetMd5Hash(MD5.Create(), Password);
        }

    }
}
