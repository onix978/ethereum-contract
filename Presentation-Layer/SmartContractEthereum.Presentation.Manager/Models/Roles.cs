using SmartContractEthereum.Domain.Interfaces.Repositories.Manager;
using SmartContractEthereum.Infrastructure.Data.Repository.Manager;
using System;
using System.Linq;
using System.Web.Security;

namespace SmartContractEthereum.Presentation.Manager.Models
{
    public class Roles : RoleProvider
    {
        private readonly IAccountRepository _accountRepository;

        public Roles()
        {
            _accountRepository = new AccountRepository();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string login)
        {
            string roles = _accountRepository.Get(x => x.Login == login).FirstOrDefault().Login;
            string[] result = { roles };

            return result;
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string login, string roleName)
        {
            var account = _accountRepository.Get(x => x.Login == login).FirstOrDefault();

            return account.Login.Equals(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}