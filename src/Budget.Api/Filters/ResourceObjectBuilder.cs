using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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

        private static bool IsAttribute(PropertyInfo prop)
        {
            var type = prop.PropertyType;
            var typeInfo = type.GetTypeInfo();

            return typeInfo.IsValueType || type == typeof(string);
        }

        private static bool IsForeignId(PropertyInfo prop) => Regex.IsMatch(prop.Name, "^.+Id$");

        public ResourceObjectBuilder TakeAttributes()
        {
            var attributes = _sourceType.GetProperties()
                .Where(IsAttribute)
                .Where(p => p.Name != "Id" && !IsForeignId(p));

            foreach (var attr in attributes)
            {
                _obj.Attributes[attr.Name] = attr.GetValue(_source);
            }

            return this;
        }

        private static bool IsRelationship(PropertyInfo prop) => !IsAttribute(prop);

        public ResourceObjectBuilder ResolveRelationships(Func<string, string, object, string> url)
        {
            var relationships = _sourceType.GetProperties()
                .Where(IsRelationship);

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

            var ids = _sourceType.GetProperties()
                .Where(IsForeignId);

            foreach (var id in ids)
            {
                var idName = id.Name;
                idName = Regex.Replace(idName, "Id$", "");

                _obj.Relationships[idName] = new RelationshipObject
                {
                    Data = new ResourceIdentifierObject
                    {
                        Id = id.GetValue(_source),
                        Type = idName
                            .Camelize()
                            .Pluralize()
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