using System;
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
            return await _collection
                .Find(IdFilter(id as string))
                .SingleAsync();
        }

        public Task CreateAsync(T item)
        {
            return _collection.InsertOneAsync(item);
        }

        public Task UpdateAsync<TKey>(TKey id, IDictionary<string, object> props)
        {
            var update = Builders<T>.Update.Combine();

            foreach (var prop in props)
            {
                update.Set(prop.Key, prop.Value);
            }

            return _collection.UpdateOneAsync(IdFilter(id as string), update);
        }

        public Task DeleteByIdAsync<TKey>(TKey id)
        {
            return _collection.DeleteOneAsync(IdFilter(id as string));
        }
    }
}