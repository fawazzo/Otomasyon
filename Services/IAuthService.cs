using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Otomasyon.Core.Entities;

public interface IAuthService
{
    Task<Kisiler> ValidateUserAsync(string tcKimlikNumarasi, string password);
    Task<string> GetUserRoleAsync(int userId);
    Task<IdentityResult> ChangePasswordAsync(Kisiler user, string newPassword);
    Task<Kisiler> GetUserByTcKimlikNumarasiAsync(string tcKimlikNumarasi);
    Task<Kisiler> GetUserByIdAsync(int userId);
    Task<BasvuruFormu> GetBasvuruFormuByIdAsync(int id);
    Task<IdentityResult> UpdateUserPasswordAsync(Kisiler user);
}
