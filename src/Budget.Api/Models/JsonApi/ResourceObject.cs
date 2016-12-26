namespace Budget.Api.Models.JsonApi
{
    internal sealed class ResourceObject : ResourceId
    {
        public AttributesObject Attributes { get; set; } = new AttributesObject();
        public RelationshipsObject Relationships { get; set; } = new RelationshipsObject();
    }
}