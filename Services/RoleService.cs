using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Otomasyon.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RoleService> _logger;

    public RoleService(ApplicationDbContext context, ILogger<RoleService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Implementation of GetRoleCountAsync method
    public async Task<int> GetRoleCountAsync(string roleName)
    {
        return await _context.BasvuruFormu.CountAsync(b => b.Gorev == roleName);
    }

    public async Task<Dictionary<string, int>> GetAvailableRolesWithCapacityAsync()
    {
        var roleLimits = new Dictionary<string, int>
        {
            { "bina yöneticisi", 14 },
            { "bina sınav sorumlusu", 3 },
            { "bina sınav sorumlus yardimcisi", 3 },
            { "salon başkanı", 8 },
            { "engelli salon görevlisi", 2 },
            { "gözetmen", 10 },
            { "yedek görevli", 5 },
            { "evrak nakil görevlisi", 2 },
            { "bina güvenlik görevlisi", 2 },
            { "şoför", 2 }
        };

        var availableRoles = new Dictionary<string, int>();

        foreach (var role in roleLimits)
        {
            var currentCount = await GetRoleCountAsync(role.Key);

            _logger.LogInformation($"Role: {role.Key}, Current Count: {currentCount}, Limit: {role.Value}");

            var remainingCapacity = role.Value - currentCount;

            if (remainingCapacity > 0)
            {
                availableRoles.Add(role.Key, remainingCapacity);
            }
        }

        _logger.LogInformation($"Available Roles: {string.Join(", ", availableRoles.Select(r => $"{r.Key}: {r.Value}"))}");

        return availableRoles;
    }
}