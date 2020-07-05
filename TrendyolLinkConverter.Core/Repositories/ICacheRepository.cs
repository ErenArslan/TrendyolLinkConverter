using System;
using System.Threading.Tasks;

namespace TrendyolLinkConverter.Core.Repositories
{
    public interface ICacheRepository<T>
    {
        Task CreateOrUpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByKeyAsync(Guid id);
    }
}
