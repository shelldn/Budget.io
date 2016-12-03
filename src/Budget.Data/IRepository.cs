using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Budget.Data
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync<TKey>(TKey id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteByIdAsync<TKey>(TKey id);
    }
}