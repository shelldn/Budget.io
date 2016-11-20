using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Budget.Api.Models
{
    [BsonIgnoreExtraElements]
    public class Category
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }

        [BsonElement("budget_id")]
        public int BudgetId { get; set; }

        [BsonElement("account_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
    }
}