using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private IEnumerable<object> GetRelationships()
        {
            var relationships = _source.GetType()
                .GetProperties()
                .Where(p => !p.PropertyType.GetTypeInfo().IsValueType && p.PropertyType != typeof(string))
                .Select(p => p.GetValue(_source))
                .Where(v => v != null);

            return relationships.SelectMany(value =>
            {
                if (value is IEnumerable)
                    return value as IEnumerable<object>;

                return new[] { value };
            });
        }

        public DocumentBuilder AddIncluded(Action<ResourceObjectBuilder> configureBuilder)
        {
            _document.Included = GetRelationships()
                .Select(o => BuildResourceObject(o, configureBuilder));

            return this;
        }

        public Document Build() => _document;
    }
}