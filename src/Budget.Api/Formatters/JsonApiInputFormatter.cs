using System;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Budget.Api.Models;
using Budget.Api.ViewModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Budget.Api.Formatters
{
    public class JsonApiInputFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
            => context.HttpContext.Request.ContentType == "application/vnd.api+json";

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;

            using (var reader = new HttpRequestStreamReader(request.Body, Encoding.UTF8))
            {
                dynamic result = new JsonSerializer().Deserialize<ExpandoObject>(new JsonTextReader(reader));

                var attrs = result.data.attributes;

                object obj;

                switch (context.ModelType.Name)
                {
                    case nameof(CategoryPatch):
                        obj = new CategoryPatch
                        {
                            Name = attrs.name
                        };
                        break;

                    case nameof(Operation):
                        obj = new Operation
                        {
                            CategoryId = Int32.Parse(result.data.relationships.category.data.id),
                            Month = Int32.Parse(result.data.relationships.month.data.id),
                            Plan = attrs.plan,
                            Fact = attrs.fact
                        };
                        break;

                    default:
                        throw new NotSupportedException($"Deserializing of type {context.ModelType} is not supported.");
                }

                return Task.FromResult(InputFormatterResult.Success(obj));
            }
        }
    }
}