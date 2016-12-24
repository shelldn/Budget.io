using Newtonsoft.Json;

namespace Budget.Api.Models.JsonApi
{
    internal sealed class RelationshipObject
    {
        public LinksObject Links { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}