using System.Collections;
using System.Collections.Generic;
using Budget.Api.Filters.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Budget.Api.Models.JsonApi;
using Budget.Api.Services;

namespace Budget.Api.Filters
{
    internal sealed class JsonApiResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            var isObjectResult = context.Result is ObjectResult;

            if (!isObjectResult) return;

            var result = (ObjectResult)context.Result;
            var obj = result.Value;

            var url = new RelationshipLinkResolver(context.GetUrlHelper());

            object data;

            if (obj is IEnumerable)
                data = ((IEnumerable<object>) obj).Select(o => BuildResourceObject(o, url));
            else
                data = BuildResourceObject(obj, url);

            context.Result = new ObjectResult(new { data });
        }

        private static ResourceObject BuildResourceObject(object obj, RelationshipLinkResolver url)
        {
            return new ResourceObjectBuilder(obj)
                .TakeId()
                .TakeType()
                .TakeAttributes()
                .ResolveRelationships(url)
                .Build();
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}