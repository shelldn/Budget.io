using Budget.Data.Models;
using Budget.Data.Operations;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Budget.Data
{
    public static class DataServiceCollectionExtensions
    {
        public static void AddBudgetData(this IServiceCollection services, string connectionString)
        {
            services.AddTransient(typeof(IRepository<>), typeof(MongoRepository<>));
            services.AddTransient<IPlanPatcher, PlanPatcher>();

            var pack = new ConventionPack { new CamelCaseElementNameConvention() };

            ConventionRegistry.Register("CamelCase", pack, t => true);

            services.AddTransient<IMongoClient>(_ => new MongoClient(connectionString));
            services.AddTransient(sp => sp.GetService<IMongoClient>().GetDatabase("budgetio"));

            services.AddTransient(sp => sp.GetService<IMongoDatabase>().GetCollection<Account>("accounts"));
            services.AddTransient(sp => sp.GetService<IMongoDatabase>().GetCollection<Category>("categories"));
            services.AddTransient(sp => sp.GetService<IMongoDatabase>().GetCollection<Operation>("operations"));
        }
    }
}
