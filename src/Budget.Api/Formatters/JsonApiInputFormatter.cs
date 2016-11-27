using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Budget.Api.Formatters
{
    public class JsonApiInputFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
            => context.HttpContext.Request.ContentType == "application/vnd.api+json";

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            throw new NotSupportedException($"Deserializing of type {context.ModelType} is not supported.");
        }
    }
}