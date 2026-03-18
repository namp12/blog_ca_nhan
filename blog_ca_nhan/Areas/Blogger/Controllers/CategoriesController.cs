using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;
using System.Security.Claims;

namespace blog_ca_nhan.Areas.Blogger.Controllers;

[Area("Blogger")]
[Authorize(AuthenticationSchemes = "CookieAuth")]
public class CategoriesController : Controller
{
    private readonly PremiumBlogDbContext _context;
    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public CategoriesController(PremiumBlogDbContext context)
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
        ViewData["ActiveMenu"] = "categories";
        var blogId = await GetUserDefaultBlogId();
        if (blogId == null) return RedirectToAction("CreateBlog", "Dashboard");

        var categories = await _context.Categories
            .Where(c => c.BlogId == blogId)
            .OrderBy(c => c.OrderIndex)
            .ToListAsync();

        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name, string? description)
    {
        var blogId = await GetUserDefaultBlogId();
        if (blogId == null) return NotFound();

        var category = new Category
        {
            BlogId = blogId.Value,
            Name = name,
            Description = description,
            Slug = name.ToLower().Replace(" ", "-"),
            OrderIndex = 0
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Đã thêm chuyên mục mới.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var blogId = await GetUserDefaultBlogId();
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.BlogId == blogId);
        
        if (category != null)
        {
            // Check if any posts are using this category
            if (await _context.Posts.AnyAsync(p => p.CategoryId == id))
            {
                TempData["Error"] = "Không thể xoá chuyên mục đang có bài viết.";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xoá chuyên mục.";
            }
        }
        return RedirectToAction("Index");
    }
}
