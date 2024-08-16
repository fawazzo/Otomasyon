using Otomasyon.Core.Entities;

namespace Otomasyon.Core.Repositories
{
    public interface IBasvuruFormuRepository : IRepository<BasvuruFormu>
    {
        // Add any specific methods for BinaSinavSorumlusu if needed

        Task<BasvuruFormu> GetByTcKimlikNumarasiAsync(string tcKimlikNumarasi);
        Task<BasvuruFormu> GetUserByTcKimlikNumarasiAsync(string tcKimlikNumarasi);
    }

}
