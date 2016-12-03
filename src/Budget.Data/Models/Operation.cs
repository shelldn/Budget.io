using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Budget.Data.Models
{
    public class Operation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("budget_id")]
        public int BudgetId { get; set; }

        [BsonElement("account_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }

        [BsonElement("category_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        public int Month { get; set; }
        public decimal Plan { get; set; }
        public decimal Fact { get; set; }
    }
}