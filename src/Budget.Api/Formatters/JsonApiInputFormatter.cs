using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Budget.Api.Models.JsonApi;
using Humanizer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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
                    var serializer = new JsonSerializer
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };

                    var payload = serializer.Deserialize<Payload>(reader);

                    var modelType = context.ModelType;
                    var model = Activator.CreateInstance(modelType);

                    modelType.GetProperty("Id").SetValue(model, payload.Data.Id);

                    foreach (var attr in payload.Data.Attributes)
                    {
                        modelType.GetProperty(attr.Key.Pascalize()).SetValue(model, attr.Value);
                    }

                    foreach (var relationship in payload.Data.Relationships)
                    {
                        var resourceId = (JObject) relationship.Value.Data;

                        var prop = modelType
                            .GetProperty($"{relationship.Key.Pascalize()}Id");

                        prop.SetValue(model, resourceId["id"].ToObject(prop.PropertyType));
                    }

                    return InputFormatterResult.SuccessAsync(model);
                }
            }
        }
    }
}