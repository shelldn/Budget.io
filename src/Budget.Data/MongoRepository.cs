﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Budget.Data
{
    public sealed class MongoRepository<T> : IRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        private static BsonDocument IdFilter(string id)
        {
            if (id == null)
                throw new ArgumentNullException();

            return new BsonDocument
            {
                ["_id"] = ObjectId.Parse(id)
            };
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync<TKey>(TKey id)
        {
            if (!(id is string))
                throw new NotImplementedException();

            return await _collection
                .Find(IdFilter(id as string))
                .SingleAsync();
        }

        public Task CreateAsync(T item)
        {
            return _collection.InsertOneAsync(item);
        }

        public Task UpdateAsync<TKey>(TKey id, T item)
        {
            if (!(id is string))
                throw new NotImplementedException();

            var q = new BsonDocument
            {
                ["_id"] = id as string
            };

            return _collection.ReplaceOneAsync(q, item);
        }

        public Task DeleteByIdAsync<TKey>(TKey id)
        {
            if (!(id is string))
                throw new NotImplementedException();

            var q = new BsonDocument
            {
                ["_id"] = id as string
            };

            return _collection.DeleteOneAsync(q);
        }
    }
}