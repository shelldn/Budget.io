using Budget.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Budget.Data
{
    public static class DataServiceCollectionExtensions
    {
        public static void AddBudgetData(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(MongoRepository<>));

            services.AddTransient<IMongoClient>(
                sp => new MongoClient(sp.GetService<IConfiguration>().GetConnectionString("DefaultConnection")));

            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            services.AddTransient(sp => sp.GetService<IMongoClient>().GetDatabase("budgetio"));

            services.AddTransient(sp => sp.GetService<IMongoDatabase>().GetCollection<Category>("categories"));
            services.AddTransient(sp => sp.GetService<IMongoDatabase>().GetCollection<Operation>("operations"));
        }
    }
}