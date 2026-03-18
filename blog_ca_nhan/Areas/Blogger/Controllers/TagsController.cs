using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;
using System.Security.Claims;

namespace blog_ca_nhan.Areas.Blogger.Controllers;

[Area("Blogger")]
[Authorize(AuthenticationSchemes = "CookieAuth")]
public class TagsController : Controller
{
    private readonly PremiumBlogDbContext _context;
    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public TagsController(PremiumBlogDbContext context)
    {
        _context = context;
    }

    private async Task<int?> GetUserDefaultBlogId()
    {
        return await _context.Blogs
            .Where(b => b.OwnerId == CurrentUserId)
            .Select(b => (int?)b.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ActiveMenu"] = "tags";
        var blogId = await GetUserDefaultBlogId();
        if (blogId == null) return RedirectToAction("CreateBlog", "Dashboard");

        var tags = await _context.Tags
            .Where(t => t.BlogId == blogId)
            .ToListAsync();

        return View(tags);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        var blogId = await GetUserDefaultBlogId();
        if (blogId == null) return NotFound();

        if (await _context.Tags.AnyAsync(t => t.BlogId == blogId && t.Name == name))
        {
            TempData["Error"] = "Thẻ này đã tồn tại.";
            return RedirectToAction("Index");
        }

        var tag = new Tag
        {
            BlogId = blogId.Value,
            Name = name,
            Slug = name.ToLower().Replace(" ", "-")
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã thêm thẻ mới.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var blogId = await GetUserDefaultBlogId();
        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.BlogId == blogId);
        
        if (tag != null)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã xoá thẻ.";
        }
        return RedirectToAction("Index");
    }
}
