using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Models;

namespace iAgentDataTool.ScriptHelpers.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindWithGuidAsync(Guid key);
        Task<IEnumerable<T>> FindWith2GuidsAsync(Guid key, Guid secondKey);
        Task<IEnumerable<T>> FindWithIdAsync(int id);
        Task<IEnumerable<T>> FindByName(string name);
        Task<IEnumerable<T>> Find(T obj);
        Task<bool> UpdateLocationKey(Guid clientKey, Guid oldLocationKey, Guid newLocationKey);
        Task<Guid> AddAsync(IEnumerable<T> entity);
        Task AddMultipleToProd(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
