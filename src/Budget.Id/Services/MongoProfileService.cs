using System.Threading.Tasks;
using Budget.Data;
using Budget.Data.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Budget.Id.Services
{
    public class MongoProfileService : IProfileService
    {
        private readonly IRepository<Account> _accounts;

        public MongoProfileService(IRepository<Account> accounts)
        {
            _accounts = accounts;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            throw new System.NotImplementedException();
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}