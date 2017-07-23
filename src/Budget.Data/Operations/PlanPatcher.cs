using Budget.Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Budget.Data.Operations
{
    internal sealed class PlanPatcher : IPlanPatcher
    {
        private readonly IMongoCollection<Operation> _operations;

        public PlanPatcher(IMongoCollection<Operation> operations)
        {
            _operations = operations;                    
        }

        public void Patch(string id, string accountId, int @value)
        {
            var update = Builders<Operation>.Update
                .Set(o => o.Plan, @value);

            var idFilter = new BsonDocument
            {
                ["_id"] = ObjectId.Parse(id),
                ["account_id"] = ObjectId.Parse(accountId)
            };

            _operations.UpdateOne(idFilter, update);
        }        
    }
}
