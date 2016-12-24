using System.Collections.Generic;

namespace Budget.Api.Models.JsonApi
{
    internal sealed class ResourceObject
    {
        public object Id { get; set; }
        public string Type { get; set; }
        public AttributesObject Attributes { get; set; } = new AttributesObject();
        public RelationshipsObject Relationships { get; set; } = new RelationshipsObject();
    }
}