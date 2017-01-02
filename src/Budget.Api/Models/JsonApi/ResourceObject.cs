namespace Budget.Api.Models.JsonApi
{
    public sealed class ResourceObject : ResourceId
    {
        public AttributesObject Attributes { get; set; } = new AttributesObject();
        public RelationshipsObject Relationships { get; set; } = new RelationshipsObject();
    }
}