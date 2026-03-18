using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;

namespace blog_ca_nhan.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "CookieAuth", Policy = "PlatformAdminOnly")]
public class ThemesController : Controller
{
    private readonly PremiumBlogDbContext _context;
    public ThemesController(PremiumBlogDbContext context) => _context = context;

    public async Task<IActionResult> Index()
        => View(await _context.Themes.OrderByDescending(t => t.Id).ToListAsync());

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(string name, string? previewImageUrl, string? description, bool isPremium)
    {
        _context.Themes.Add(new Theme
        {
            Name = name,
            CodeName = name.ToLower().Replace(" ", "-"),
            PreviewImageUrl = previewImageUrl,
            Description = description,
            IsPremium = isPremium,
            IsActive = true
        });
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã thêm chủ đề thành công!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var theme = await _context.Themes.FindAsync(id);
        if (theme != null)
        {
            theme.IsActive = !(theme.IsActive ?? true);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var theme = await _context.Themes.FindAsync(id);
        if (theme != null) { _context.Themes.Remove(theme); await _context.SaveChangesAsync(); }
        return RedirectToAction("Index");
    }
}
