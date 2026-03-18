using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;

namespace blog_ca_nhan.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "CookieAuth", Policy = "PlatformAdminOnly")]
public class BlogsController : Controller
{
    private readonly PremiumBlogDbContext _context;
    public BlogsController(PremiumBlogDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Blogs
            .Include(b => b.Owner)
            .Include(b => b.Plan)
            .Include(b => b.Posts)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => b.Title.Contains(search) || b.Subdomain.Contains(search));

        ViewData["Search"] = search;
        return View(await query.OrderByDescending(b => b.CreatedAt).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog != null)
        {
            blog.Status = blog.Status == 1 ? 0 : 1;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog != null)
        {
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
