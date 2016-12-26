namespace Budget.Api.Services
{
    public interface IRelationshipLinkResolver
    {
        string GetRelated(string sourceName, string relationshipName);
    }
}