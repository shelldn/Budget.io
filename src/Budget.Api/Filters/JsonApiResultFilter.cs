using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Budget.Api.Filters
{
    internal sealed class JsonApiResultFilter : IResultFilter
    {
        private readonly ILogger _log;

        public JsonApiResultFilter(ILogger<JsonApiResultFilter> log)
        {
            _log = log;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var isObjectResult = context.Result is ObjectResult;

            _log.LogInformation("Is ObjectResult: {0}", isObjectResult);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}