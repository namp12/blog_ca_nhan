using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;

namespace blog_ca_nhan.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "CookieAuth", Policy = "PlatformAdminOnly")]
public class PostsController : Controller
{
    private readonly PremiumBlogDbContext _context;
    public PostsController(PremiumBlogDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Blog)
            .Include(p => p.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Title.Contains(search) || p.Author.Username.Contains(search));

        ViewData["Search"] = search;
        return View(await query.OrderByDescending(p => p.CreatedAt).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã xoá bài viết thành công.";
        }
        return RedirectToAction("Index");
    }
}
