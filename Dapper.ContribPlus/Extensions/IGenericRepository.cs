using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.ContribPlus.Extensions
{
     public interface IGenericRepository<T> where T : class
    {
        void Delete(T model);
        Task DeleteAsync(T model);
        void DeleteAll();
        Task DeleteAllAsync();
        T Get(object id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetPaginated(ref int total, int currentPage, int itemsPerPage);
        Task<(IEnumerable<T> list,int total)> GetPaginatedAsync(int currentPage, int itemsPerPage);
        Task<T> GetTaskAsync(object id);
        void Insert(T model);
        Task InsertAsync(T model);
        void BulkInsert(IEnumerable<T> model,int batchSize=0 ,int timeout=30);
        Task BulkInsertAsync(IEnumerable<T> model,int batchSize=0 ,int timeout=30);
        void Update(T model);
        Task UpdateAsync(T model);

    }
}