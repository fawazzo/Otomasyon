using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRoleService
{
    Task<Dictionary<string, int>> GetAvailableRolesWithCapacityAsync();
    Task<int> GetRoleCountAsync(string roleName);
}
