using Microsoft.EntityFrameworkCore;
using Otomasyon.Core.Entities;

namespace Otomasyon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BasvuruFormu> BasvuruFormu { get; set; }
        public DbSet<Kisiler> Kisiler { get; set; }
        public DbSet<MevcutSinavlar> MevcutSinavlar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BasvuruFormu>()
                .ToTable("BasvuruFormu");

            modelBuilder.Entity<BasvuruFormu>()
                .HasOne(b => b.MevcutSinav)
                .WithMany()  // Adjust if MevcutSinavlar should have a collection of BasvuruFormu
                .HasForeignKey(b => b.MevcutSinavId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
