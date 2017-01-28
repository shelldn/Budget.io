using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Budget.Data.Models
{
    public class Account
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }
    }
}