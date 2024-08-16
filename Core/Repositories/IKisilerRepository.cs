using Otomasyon.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otomasyon.Core.Repositories
{
    public interface IKisilerRepository
    {
        Task<IEnumerable<Kisiler>> ListAllAsync();
        Task<Kisiler> GetByIdAsync(int id);
        Task<Kisiler> GetKisiByTcAsync(string tcKimlikNumarasi);
        Task AddAsync(Kisiler entity);
        Task UpdateAsync(Kisiler entity);
        Task DeleteAsync(Kisiler entity);
    }
}
