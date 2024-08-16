using Microsoft.AspNetCore.Mvc;

public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        // Retrieve the role capacities
        var availableRolesWithCapacity = await _roleService.GetAvailableRolesWithCapacityAsync();
        ViewBag.AvailableRolesWithCapacity = availableRolesWithCapacity;

        return View();
    }
}
