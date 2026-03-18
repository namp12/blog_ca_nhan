using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;
using System.Security.Claims;

namespace blog_ca_nhan.Controllers;

[Authorize(AuthenticationSchemes = "CookieAuth")]
public class DashboardController : Controller
{
    private readonly PremiumBlogDbContext _context;
    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public DashboardController(PremiumBlogDbContext context)
    {
        _context = context;
    }

    // ═══ BLOGS (Trang web) ═══
    public async Task<IActionResult> Blogs()
    {
        ViewData["ActiveMenu"] = "blogs";
        var blogs = await _context.Blogs
            .Where(b => b.OwnerId == CurrentUserId)
            .Include(b => b.Plan)
            .Include(b => b.Posts)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
        return View(blogs);
    }

    [HttpGet]
    public async Task<IActionResult> CreateBlog()
    {
        ViewData["ActiveMenu"] = "blogs";
        ViewData["Plans"] = await _context.SubscriptionPlans.ToListAsync();
        ViewData["Themes"] = await _context.Themes.Where(t => t.IsActive == true).ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog(string title, string subdomain, string? description)
    {
        // Check subdomain uniqueness
        if (await _context.Blogs.AnyAsync(b => b.Subdomain == subdomain))
        {
            TempData["Error"] = "Subdomain này đã tồn tại. Vui lòng chọn tên khác.";
            return RedirectToAction("CreateBlog");
        }

        var freePlan = await _context.SubscriptionPlans.FirstOrDefaultAsync();
        var defaultTheme = await _context.Themes.FirstOrDefaultAsync(t => t.IsActive == true);

        if (freePlan == null || defaultTheme == null)
        {
            TempData["Error"] = "Hệ thống chưa được thiết lập. Vui lòng liên hệ Admin.";
            return RedirectToAction("Blogs");
        }

        var blog = new Blog
        {
            Title = title,
            Subdomain = subdomain.ToLower().Trim(),
            Description = description,
            OwnerId = CurrentUserId,
            PlanId = freePlan.Id,
            CurrentThemeId = defaultTheme.Id,
            CreatedAt = DateTime.Now,
            Status = 1
        };

        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Blog \"{title}\" đã được tạo thành công!";
        return RedirectToAction("Blogs");
    }

    // ═══ POSTS (Bài viết) ═══
    public async Task<IActionResult> Posts(int? blogId)
    {
        ViewData["ActiveMenu"] = "posts";
        var myBlogs = await _context.Blogs
            .Where(b => b.OwnerId == CurrentUserId)
            .ToListAsync();

        ViewData["Blogs"] = myBlogs;
        var selectedBlogId = blogId ?? myBlogs.FirstOrDefault()?.Id;
        ViewData["SelectedBlogId"] = selectedBlogId;

        if (selectedBlogId == null) return View(new List<Post>());

        var posts = await _context.Posts
            .Where(p => p.BlogId == selectedBlogId)
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return View(posts);
    }

    [HttpGet]
    public async Task<IActionResult> CreatePost(int blogId)
    {
        ViewData["ActiveMenu"] = "posts";
        ViewData["BlogId"] = blogId;
        ViewData["Categories"] = await _context.Categories
            .Where(c => c.BlogId == blogId)
            .ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(int blogId, string title, string content, string? summary, int? categoryId, bool publish = false)
    {
        // Ensure a default category exists
        var category = categoryId.HasValue
            ? await _context.Categories.FindAsync(categoryId.Value)
            : await _context.Categories.FirstOrDefaultAsync(c => c.BlogId == blogId);

        if (category == null)
        {
            // Auto-create a General category
            category = new Category { BlogId = blogId, Name = "Chung", Slug = "chung" };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        var slug = title.ToLower()
            .Replace(" ", "-")
            .Replace("đ", "d")
            .Replace("ă", "a").Replace("â", "a").Replace("ê", "e").Replace("ô", "o").Replace("ư", "u").Replace("ơ", "o");

        var post = new Post
        {
            BlogId = blogId,
            AuthorId = CurrentUserId,
            Title = title,
            Content = content,
            Summary = summary,
            Slug = slug + "-" + DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            CategoryId = category.Id,
            Status = publish ? 1 : 0,
            PublishedAt = publish ? DateTime.Now : null,
            CreatedAt = DateTime.Now
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Bài viết đã được lưu thành công!";
        return RedirectToAction("Posts", new { blogId });
    }

    [HttpPost]
    public async Task<IActionResult> DeletePost(int id, int blogId)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null && post.BlogId == blogId)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Posts", new { blogId });
    }

    // ═══ DOMAINS (Tên miền) ═══
    public async Task<IActionResult> Domains()
    {
        ViewData["ActiveMenu"] = "domains";
        var blogs = await _context.Blogs
            .Where(b => b.OwnerId == CurrentUserId)
            .ToListAsync();
        return View(blogs);
    }

    // ═══ THEMES (Chủ đề) ═══
    public async Task<IActionResult> Themes()
    {
        ViewData["ActiveMenu"] = "themes";
        var themes = await _context.Themes
            .Where(t => t.IsActive == true)
            .ToListAsync();
        var myBlog = await _context.Blogs.FirstOrDefaultAsync(b => b.OwnerId == CurrentUserId);
        ViewData["CurrentThemeId"] = myBlog?.CurrentThemeId;
        return View(themes);
    }

    [HttpPost]
    public async Task<IActionResult> ApplyTheme(int themeId, int blogId)
    {
        var blog = await _context.Blogs
            .FirstOrDefaultAsync(b => b.Id == blogId && b.OwnerId == CurrentUserId);
        if (blog != null)
        {
            blog.CurrentThemeId = themeId;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã áp dụng chủ đề mới!";
        }
        return RedirectToAction("Themes");
    }

    // ═══ PLUGINS ═══
    public IActionResult Plugins()
    {
        ViewData["ActiveMenu"] = "plugins";
        return View();
    }

    // ═══ SETTINGS ═══
    public async Task<IActionResult> Settings()
    {
        ViewData["ActiveMenu"] = "settings";
        var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.OwnerId == CurrentUserId);
        return View(blog);
    }
}
