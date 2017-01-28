using System.Linq;
using System.Threading.Tasks;
using Budget.Data;
using Budget.Data.Models;
using IdentityModel;
using IdentityServer4.Validation;

namespace Budget.Id.Services
{
    public class MongoResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IRepository<Account> _accounts;

        public MongoResourceOwnerPasswordValidator(IRepository<Account> accounts)
        {
            _accounts = accounts;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var q = _accounts.QueryAsync(a =>
                a.UserName == context.UserName &&
                a.Password == context.Password);

            var account = (await q).FirstOrDefault();

            if (account != null)
                context.Result = new GrantValidationResult(account.Id, OidcConstants.AuthenticationMethods.Password);
        }
    }
}