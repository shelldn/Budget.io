using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Budget.Id.Services
{
    public class MongoProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}