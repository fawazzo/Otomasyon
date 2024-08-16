using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Otomasyon.Core.Entities;
using Otomasyon.Infrastructure.Data;

namespace Otomasyon.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IPasswordHasher<Kisiler> _passwordHasher;

        public AuthService(ApplicationDbContext context, ILogger<AuthService> logger, IPasswordHasher<Kisiler> passwordHasher)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<Kisiler> ValidateUserAsync(string tcKimlikNumarasi, string password)
        {
            _logger.LogInformation("Validating user with TcKimlikNumarasi: {TcKimlikNumarasi}", tcKimlikNumarasi);

            var user = await _context.Kisiler
                .FirstOrDefaultAsync(u => u.TcKimlikNumarasi == tcKimlikNumarasi);

            if (user != null)
            {
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    _logger.LogInformation("User found: {UserId}", user.Id);
                    return user;
                }
            }

            _logger.LogWarning("User not found or password incorrect for TcKimlikNumarasi: {TcKimlikNumarasi}", tcKimlikNumarasi);
            return null;
        }


        public async Task<string> GetUserRoleAsync(int userId)
        {
            var user = await _context.Kisiler.FindAsync(userId);
            return user?.Unvan;
        }

        public async Task<IdentityResult> ChangePasswordAsync(Kisiler user, string newPassword)
        {
            // Hash the new password
            var hashedPassword = _passwordHasher.HashPassword(user, newPassword);

            // Update the user’s password
            user.Password = hashedPassword;
            _context.Kisiler.Update(user);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }



        public async Task<Kisiler> GetUserByTcKimlikNumarasiAsync(string tcKimlikNumarasi)
        {
            return await _context.Kisiler.FirstOrDefaultAsync(u => u.TcKimlikNumarasi == tcKimlikNumarasi);
        }

        public async Task<Kisiler> GetUserByIdAsync(int userId)
        {
            return await _context.Kisiler.FindAsync(userId);
        }

        public async Task CreateUserAsync(Kisiler user)
        {
            _context.Kisiler.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IdentityResult> UpdateUserPasswordAsync(Kisiler user)
        {
            // Update password logic here
            _context.Update(user);
            return await _context.SaveChangesAsync() > 0
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<BasvuruFormu> GetBasvuruFormuByIdAsync(int id)
        {
            return await _context.BasvuruFormu.FindAsync(id);
        }
    }
}
