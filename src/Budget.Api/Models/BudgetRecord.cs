using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Budget.Api.Models
{
    [BsonIgnoreExtraElements]
    public class BudgetRecord
    {
        public int Year { get; set; }

        [BsonElement("account_id")]
        public int AccountId { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}