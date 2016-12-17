using Microsoft.AspNetCore.Mvc.Filters;

namespace Budget.Api.Filters
{
    public class JsonApiResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Request.Headers["Accept"] = "application/vnd.api+json";
            throw new System.NotImplementedException();
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}