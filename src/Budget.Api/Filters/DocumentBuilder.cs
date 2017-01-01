using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Budget.Api.Models.JsonApi;

namespace Budget.Api.Filters
{
    internal sealed class DocumentBuilder
    {
        private readonly object _source;
        private readonly Document _document;

        public DocumentBuilder(object source)
        {
            _source = source;
            _document = new Document();
        }

        private static bool IsArray(object obj)
        {
            return obj is IEnumerable;
        }

        private static ResourceObject BuildResourceObject(object obj, Action<ResourceObjectBuilder> configureBuilder)
        {
            var builder = new ResourceObjectBuilder(obj);
            configureBuilder(builder);
            return builder.Build();
        }

        public DocumentBuilder AddData(Action<ResourceObjectBuilder> configureBuilder)
        {
            if (IsArray(_source))
            {
                var array = _source as IEnumerable<object>;

                _document.Data = array.Select(o => BuildResourceObject(o, configureBuilder));

                return this;
            }

            _document.Data = BuildResourceObject(_source, configureBuilder);

            return this;
        }

        public Document Build() => _document;
    }
}