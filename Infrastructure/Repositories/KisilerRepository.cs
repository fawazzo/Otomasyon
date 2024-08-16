using Microsoft.EntityFrameworkCore;
using Otomasyon.Core.Entities;
using Otomasyon.Core.Repositories;
using Otomasyon.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otomasyon.Infrastructure.Repositories
{
    public class KisilerRepository : IKisilerRepository
    {
        private readonly ApplicationDbContext _context;

        public KisilerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kisiler>> ListAllAsync()
        {
            return await _context.Kisiler.ToListAsync();
        }

        public async Task<Kisiler> GetByIdAsync(int id)
        {
            return await _context.Kisiler.FindAsync(id);
        }

        public async Task<Kisiler> GetKisiByTcAsync(string tcKimlikNumarasi)
        {
            return await _context.Kisiler
                .FirstOrDefaultAsync(k => k.TcKimlikNumarasi == tcKimlikNumarasi);
        }

        public async Task AddAsync(Kisiler entity)
        {
            await _context.Kisiler.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Kisiler entity)
        {
            _context.Kisiler.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Kisiler entity)
        {
            _context.Kisiler.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
