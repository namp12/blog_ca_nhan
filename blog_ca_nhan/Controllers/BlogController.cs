using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;

namespace blog_ca_nhan.Controllers;

public class BlogController : Controller
{
    private readonly PremiumBlogDbContext _context;

    public BlogController(PremiumBlogDbContext context)
    {
        _context = context;
    }

    // [GET] /b/{subdomain}
    [Route("b/{subdomain}")]
    public async Task<IActionResult> Index(string subdomain)
    {
        var blog = await _context.Blogs
            .Include(b => b.Owner)
            .Include(b => b.CurrentTheme)
            .Include(b => b.Categories)
            .Include(b => b.Posts.Where(p => p.Status == 1).OrderByDescending(p => p.PublishedAt))
            .FirstOrDefaultAsync(b => b.Subdomain == subdomain);

        if (blog == null) return NotFound("Blog không tồn tại.");

        return View(blog);
    }

    // [GET] /b/{subdomain}/{slug}
    [Route("b/{subdomain}/{slug}")]
    public async Task<IActionResult> Details(string subdomain, string slug)
    {
        var blog = await _context.Blogs
            .Include(b => b.Owner)
            .Include(b => b.CurrentTheme)
            .Include(b => b.Categories)
            .FirstOrDefaultAsync(b => b.Subdomain == subdomain);

        if (blog == null) return NotFound("Blog không tồn tại.");

        var post = await _context.Posts
            .Include(p => p.Category)
            .Include(p => p.Author)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.BlogId == blog.Id && p.Slug == slug && p.Status == 1);

        if (post == null) return NotFound("Bài viết không tồn tại.");

        ViewData["Blog"] = blog;
        return View(post);
    }

    // [POST] /Blog/PostComment
    [HttpPost]
    public async Task<IActionResult> PostComment(int postId, string authorName, string authorEmail, string content, string subdomain, string slug)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null) return NotFound();

        var comment = new Comment
        {
            PostId = postId,
            BlogId = post.BlogId,
            AuthorName = authorName,
            AuthorEmail = authorEmail,
            Content = content,
            CreatedAt = DateTime.Now,
            IsApproved = false // Require moderation by default
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        TempData["CommentSuccess"] = "Cảm ơn bạn! Bình luận của bạn đã được gửi và đang chờ duyệt.";
        return Redirect($"/b/{subdomain}/{slug}#comments");
    }
}
