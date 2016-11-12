using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Budget.Api.Models
{
    [BsonIgnoreExtraElements]
    public class Operation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("budget_id")]
        public int BudgetId { get; set; }

        [BsonElement("category_id")]
        public int CategoryId { get; set; }

        public int Month { get; set; }
        public decimal Plan { get; set; }
        public decimal Fact { get; set; }
    }
}