using System.Text;
using System.Threading.Tasks;
using Budget.Api.Models.JsonApi;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace Budget.Api.Formatters
{
    public class JsonApiInputFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            return context.HttpContext.Request.ContentType == "application/vnd.api+json";
        }

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            using (var txt = context.ReaderFactory(request.Body, Encoding.UTF8))
            {
                using (var reader = new JsonTextReader(txt))
                {
                    var serializer = new JsonSerializer();
                    var payload = serializer.Deserialize<Payload>(reader);

                    return InputFormatterResult.SuccessAsync(payload.Data);
                }
            }
        }
    }
}