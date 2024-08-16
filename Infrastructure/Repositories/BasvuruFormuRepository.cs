using Microsoft.EntityFrameworkCore;
using Otomasyon.Core.Entities;
using Otomasyon.Core.Repositories;
using Otomasyon.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otomasyon.Infrastructure.Repositories
{
    public class BasvuruFormuRepository : IBasvuruFormuRepository
    {
        private readonly ApplicationDbContext _context;

        public BasvuruFormuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BasvuruFormu> GetByIdAsync(int id)
        {
            return await _context.BasvuruFormu
                .Include(b => b.MevcutSinav) // Include related MevcutSinav if needed
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IReadOnlyList<BasvuruFormu>> ListAllAsync()
        {
            return await _context.BasvuruFormu
                .Include(b => b.MevcutSinav) // Include related MevcutSinav
                .ToListAsync();
        }

        public async Task<BasvuruFormu> AddAsync(BasvuruFormu entity)
        {
            var result = await _context.BasvuruFormu.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task UpdateAsync(BasvuruFormu entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BasvuruFormu entity)
        {
            _context.BasvuruFormu.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<BasvuruFormu> GetByTcKimlikNumarasiAsync(string tcKimlikNumarasi)
        {
            return await _context.BasvuruFormu
                .FirstOrDefaultAsync(bf => bf.TcKimlikNumarasi == tcKimlikNumarasi);
        }

        public async Task<BasvuruFormu> GetUserByTcKimlikNumarasiAsync(string tcKimlikNumarasi)
        {
            var user = await _context.Kisiler
                .Where(u => u.TcKimlikNumarasi == tcKimlikNumarasi)
                .Select(u => new BasvuruFormu
                {
                    TcKimlikNumarasi = u.TcKimlikNumarasi,
                    Adi = u.Adi,
                    Soyadi = u.Soyadi,
                    Unvan = u.Unvan
                })
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
