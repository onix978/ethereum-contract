using SmartContractEthereum.Domain.Entities;
using SmartContractEthereum.Domain.Entities.Manager;
using SmartContractEthereum.Infrastructure.Data.Persistence;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SmartContractEthereum.Infrastructure.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SmartContractEthereumContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator());
        }

        protected override void Seed(SmartContractEthereumContext context)
        {
            DeleteData<EthereumContract>(context);

            Account account = new Account() { Login = "admin", Password = "admin" };
            account.EncryptPassword();

            context.Account.AddOrUpdate(
               account
            );
        }

        private void DeleteData<T>(SmartContractEthereumContext context) where T : class
        {
            var entity = context.Set<T>().ToList();

            entity.ForEach(item => context.Set<T>().Remove(item));

            context.SaveChanges();
        }
    }
}
