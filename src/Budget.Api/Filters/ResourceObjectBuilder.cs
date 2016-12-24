using System;
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

        public ResourceObject Build()
        {
            return _obj;
        }
    }
}