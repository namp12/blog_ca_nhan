using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;

namespace blog_ca_nhan.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "CookieAuth", Policy = "PlatformAdminOnly")]
public class UsersController : Controller
{
    private readonly PremiumBlogDbContext _context;
    public UsersController(PremiumBlogDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Users.Include(u => u.Blogs).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u => u.Username.Contains(search) || u.Email.Contains(search));
        ViewData["Search"] = search;
        return View(await query.OrderByDescending(u => u.CreatedAt).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            user.IsActive = !(user.IsActive ?? true);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> MakeAdmin(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            user.IsPlatformAdmin = !(user.IsPlatformAdmin ?? false);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
