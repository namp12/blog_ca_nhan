using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;
using System.Security.Claims;

namespace blog_ca_nhan.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "CookieAuth", Policy = "PlatformAdminOnly")]
public class HomeController : Controller
{
    private readonly PremiumBlogDbContext _context;

    public HomeController(PremiumBlogDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var stats = new AdminDashboardViewModel
        {
            TotalUsers = await _context.Users.CountAsync(),
            TotalBlogs = await _context.Blogs.CountAsync(),
            TotalPosts = await _context.Posts.CountAsync(),
            TotalComments = await _context.Comments.CountAsync(),
            RecentUsers = await _context.Users.OrderByDescending(u => u.CreatedAt).Take(5).ToListAsync(),
            RecentBlogs = await _context.Blogs.Include(b => b.Owner).OrderByDescending(b => b.CreatedAt).Take(5).ToListAsync(),
        };
        return View(stats);
    }
}

public class AdminDashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalBlogs { get; set; }
    public int TotalPosts { get; set; }
    public int TotalComments { get; set; }
    public List<User> RecentUsers { get; set; } = new();
    public List<Blog> RecentBlogs { get; set; } = new();
}
