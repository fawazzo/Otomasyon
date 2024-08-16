using Microsoft.EntityFrameworkCore;
using Otomasyon.Core.Entities;
using Otomasyon.Core.Repositories;
using Otomasyon.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otomasyon.Infrastructure.Repositories
{
    public class MevcutSinavlarRepository : IMevcutSinavlarRepository
    {
        private readonly ApplicationDbContext _context;

        public MevcutSinavlarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MevcutSinavlar>> ListAllAsync()
        {
            return await _context.MevcutSinavlar.ToListAsync();
        }

        public async Task<MevcutSinavlar> GetByIdAsync(int id)
        {
            return await _context.MevcutSinavlar.FindAsync(id);
        }

        public async Task<MevcutSinavlar> AddAsync(MevcutSinavlar entity)
        {
            var result = await _context.MevcutSinavlar.AddAsync(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task UpdateAsync(MevcutSinavlar entity)
        {
            _context.MevcutSinavlar.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.MevcutSinavlar.FindAsync(id);
            if (entity != null)
            {
                _context.MevcutSinavlar.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
