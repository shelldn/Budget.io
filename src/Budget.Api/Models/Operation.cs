using MongoDB.Bson.Serialization.Attributes;

namespace Budget.Api.Models
{
    [BsonIgnoreExtraElements]
    public class Operation
    {
        public int Id { get; set; }
        public decimal Plan { get; set; }
        public decimal Fact { get; set; }
    }
}