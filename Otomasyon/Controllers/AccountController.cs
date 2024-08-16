using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Otomasyon.Core.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountController> _logger;
    private readonly IPasswordHasher<Kisiler> _passwordHasher;

    public AccountController(IAuthService authService, ILogger<AccountController> logger, IPasswordHasher<Kisiler> passwordHasher)
    {
        _authService = authService;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Attempting to log in user with TcKimlikNumarasi: {TcKimlikNumarasi}", model.TcKimlikNumarasi);

            var user = await _authService.GetUserByTcKimlikNumarasiAsync(model.TcKimlikNumarasi);
            if (user != null)
            {
                var isPasswordValid = false;
                try
                {
                    var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
                    isPasswordValid = verificationResult == PasswordVerificationResult.Success;
                }
                catch (FormatException)
                {
                    isPasswordValid = user.Password == model.Password;
                }

                if (isPasswordValid)
                {
                    if (user.Password == model.Password)
                    {
                        user.Password = _passwordHasher.HashPassword(user, model.Password);
                        await _authService.UpdateUserPasswordAsync(user);
                    }

                    if (model.Password == "12345")
                    {
                        return RedirectToAction("ChangePassword", new { userId = user.Id });
                    }

                    _logger.LogInformation("User logged in: {UserId}, Role: {UserRole}", user.Id, user.Unvan);

                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, user.Adi + " " + user.Soyadi),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("TcKimlikNumarasi", model.TcKimlikNumarasi),
                    new Claim(ClaimTypes.Role, user.Unvan)
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    // Redirect based on job role
                    if (user.Unvan == "Admin")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Create", "BasvuruFormu");
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid login attempt for TcKimlikNumarasi: {TcKimlikNumarasi}", model.TcKimlikNumarasi);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            else
            {
                _logger.LogWarning("Invalid login attempt for TcKimlikNumarasi: {TcKimlikNumarasi}", model.TcKimlikNumarasi);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        return View(model);
    }


    [HttpGet]
    public IActionResult ChangePassword(int? userId)
    {
        // If userId is not provided, retrieve it from the logged-in user's claims
        if (userId == null)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var parsedUserId))
            {
                return Unauthorized(); // or Redirect to a proper page
            }
            userId = parsedUserId;
        }

        var model = new ChangePasswordViewModel { UserId = userId.Value };
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _authService.GetUserByIdAsync(model.UserId);
            if (user != null)
            {
                _logger.LogInformation("User found: {UserId}", user.Id);

                // Verify the current password
                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, model.CurrentPassword);
                if (verificationResult != PasswordVerificationResult.Success)
                {
                    ModelState.AddModelError(string.Empty, "The current password is incorrect.");
                    return View(model);
                }

                // Hash the new password
                user.Password = _passwordHasher.HashPassword(user, model.NewPassword);

                // Save the updated password
                var result = await _authService.UpdateUserPasswordAsync(user);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Password changed successfully for user: {UserId}", user.Id);
                    return RedirectToAction("Details", "BasvuruFormu", new { id = user.Id });
                }
                else
                {
                    _logger.LogError("Password change failed for user: {UserId}", user.Id);
                    ModelState.AddModelError(string.Empty, "Password change failed.");
                }
            }
            else
            {
                _logger.LogError("User not found for userId: {UserId}", model.UserId);
                ModelState.AddModelError(string.Empty, "User not found.");
            }
        }

        return View(model);
    }



    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var basvuruFormu = await _authService.GetBasvuruFormuByIdAsync(id);
        if (basvuruFormu == null)
        {
            return NotFound();
        }

        return View(basvuruFormu);
    }
}
