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

                var obj = new CategoryPatch
                {
                    Name = attrs.name
                };

                return Task.FromResult(InputFormatterResult.Success(obj));
            }
        }
    }
}