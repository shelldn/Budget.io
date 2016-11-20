using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Budget.Api.Models
{
    [BsonIgnoreExtraElements]
    public class Category
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}