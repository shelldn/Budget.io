using System.Collections;
using System.Collections.Generic;
using Budget.Api.Filters.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

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

            var url = context.GetUrlHelper();

            object data;

            if (obj is IEnumerable)
            {
                data = ((IEnumerable<object>)obj).Select(o => new ResourceObjectBuilder(o)
                    .TakeId()
                    .TakeType()
                    .TakeAttributes()
                    .ResolveRelationships(url.Action)
                    .Build());
            }
            else
            {
                data = new ResourceObjectBuilder(obj)
                    .TakeId()
                    .TakeType()
                    .TakeAttributes()
                    .ResolveRelationships(url.Action)
                    .Build();
            }

            context.Result = new ObjectResult(new { data });
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}