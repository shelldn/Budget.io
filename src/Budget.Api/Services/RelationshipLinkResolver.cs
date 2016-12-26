using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Services
{
    internal sealed class RelationshipLinkResolver : IRelationshipLinkResolver
    {
        private readonly IUrlHelper _url;

        public RelationshipLinkResolver(IUrlHelper url)
        {
            _url = url;
        }

        public string GetRelated(string sourceName, string relationshipName)
        {
            return _url.Action($"GetBy{sourceName}Id", relationshipName);
        }
    }
}