using System.Collections.Generic;

namespace Budget.Api.Models.JsonApi
{
    internal sealed class ResourceObject
    {
        public object Id { get; set; }
        public string Type { get; set; }
        public IDictionary<string, RelationshipObject> Relationships { get; set; } = new Dictionary<string, RelationshipObject>();
    }
}