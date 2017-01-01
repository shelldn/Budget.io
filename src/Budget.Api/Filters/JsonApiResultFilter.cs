using System;
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

            Action<ResourceObjectBuilder> configureBuilder = b => b
                .TakeId()
                .TakeType()
                .TakeAttributes()
                .ResolveRelationships(url);

            var document = new DocumentBuilder(obj)
                .AddData(configureBuilder)
                .AddIncluded(configureBuilder)
                .Build();

            context.Result = new ObjectResult(document);
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