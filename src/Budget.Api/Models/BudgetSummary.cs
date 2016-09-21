using MongoDB.Bson.Serialization.Attributes;

namespace Budget.Api.Models
{
    [BsonIgnoreExtraElements]
    public class BudgetSummary
    {
        public int Year { get; set; }
    }
}