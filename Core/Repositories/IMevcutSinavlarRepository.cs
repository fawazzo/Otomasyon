using System.Collections.Generic;
using System.Threading.Tasks;
using Otomasyon.Core.Entities;

namespace Otomasyon.Core.Repositories
{
    public interface IMevcutSinavlarRepository
    {
        Task<IEnumerable<MevcutSinavlar>> ListAllAsync();
        Task<MevcutSinavlar> GetByIdAsync(int id);
        Task<MevcutSinavlar> AddAsync(MevcutSinavlar entity);
        Task UpdateAsync(MevcutSinavlar entity);
        Task DeleteAsync(int id);  // Accepting id for deletion is more common
    }
}
