using System;
using System.Linq;
using System.Reflection;
using Budget.Api.Models.JsonApi;
using Humanizer;

namespace Budget.Api.Filters
{
    internal sealed class ResourceObjectBuilder
    {
        private readonly object _source;
        private readonly Type _sourceType;
        private readonly ResourceObject _obj;

        public ResourceObjectBuilder(object source)
        {
            _source = source;
            _sourceType = source.GetType();
            _obj = new ResourceObject();
        }

        public ResourceObjectBuilder TakeId()
        {
            _obj.Id = _sourceType.GetProperty("Id").GetValue(_source);
            return this;
        }

        public ResourceObjectBuilder TakeType()
        {
            _obj.Type = _sourceType.Name
                .Pluralize()
                .Camelize();

            return this;
        }

        public ResourceObjectBuilder TakeAttributes()
        {
            return this;
        }

        public ResourceObjectBuilder ResolveRelationships(Func<string, string, object, string> url)
        {
            var relationships = _sourceType.GetProperties().Where(p => !p.PropertyType.GetTypeInfo().IsValueType);

            foreach (var relationship in relationships)
            {
                var action = $"GetBy{_sourceType.Name}Id";
                var controller = relationship.Name;

                _obj.Relationships[relationship.Name] = new RelationshipObject
                {
                    Links = new LinksObject
                    {
                        Related = url(action, controller, new { _obj.Id })
                    }
                };
            }

            return this;
        }

        public ResourceObject Build()
        {
            return _obj;
        }
    }
}