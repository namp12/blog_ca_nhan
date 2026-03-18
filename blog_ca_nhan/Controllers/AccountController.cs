using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Controllers;

public class AccountController : Controller
{
    private readonly PremiumBlogDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountController(PremiumBlogDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string username, string email, string password, string displayName)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
            return View();
        }

        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            ModelState.AddModelError("username", "Tên đăng nhập này đã được sử dụng.");
        }
        
        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            ModelState.AddModelError("email", "Email này đã được sử dụng.");
        }

        if (!ModelState.IsValid) return View();

        var user = new User
        {
            Username = username,
            Email = email,
            DisplayName = displayName,
            CreatedAt = DateTime.Now,
            IsActive = true,
            IsPlatformAdmin = false
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đăng ký thành công! Mời bạn đăng nhập.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, bool rememberMe = false)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
            return View();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        if (user != null)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("DisplayName", user.DisplayName),
                    new Claim("IsPlatformAdmin", (user.IsPlatformAdmin == true).ToString().ToLower())
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(30) : null
                };

                await HttpContext.SignInAsync("CookieAuth", principal, authProperties);

                if (user.IsPlatformAdmin == true)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("Blogs", "Dashboard", new { area = "Blogger" });
            }
        }

        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Index", "Home");
    }
}
