using System;
using System.Collections;
using Budget.Api.Filters.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Budget.Api.Filters
{
    internal sealed class JsonApiResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            var isObjectResult = context.Result is ObjectResult;

            if (!isObjectResult) return;

            var result = (ObjectResult) context.Result;
            var obj = result.Value;

            if (obj is IEnumerable)
            {
                throw new NotImplementedException();
            }
            else
            {
                var url = context.GetUrlHelper();

                var data = new ResourceObjectBuilder(obj)
                    .TakeId()
                    .TakeType()
                    .TakeAttributes()
                    .ResolveRelationships(url.Action)
                    .Build();

                context.Result = new ObjectResult(new { data });
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}