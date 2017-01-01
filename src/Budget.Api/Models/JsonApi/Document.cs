using System.Collections.Generic;

namespace Budget.Api.Models.JsonApi
{
    internal sealed class Document
    {
        public object Data { get; set; }
        public IEnumerable<ResourceObject> Included { get; set; }
    }
}