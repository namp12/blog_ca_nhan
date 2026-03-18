using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;
using System.Security.Claims;

namespace blog_ca_nhan.Areas.Blogger.Controllers;

[Area("Blogger")]
[Authorize(AuthenticationSchemes = "CookieAuth")]
public class CommentsController : Controller
{
    private readonly PremiumBlogDbContext _context;
    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public CommentsController(PremiumBlogDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ActiveMenu"] = "comments";
        // Get all comments for all blogs owned by the current user
        var comments = await _context.Comments
            .Include(c => c.Post)
            .Include(c => c.Blog)
            .Where(c => c.Blog.OwnerId == CurrentUserId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return View(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.Blog)
            .FirstOrDefaultAsync(c => c.Id == id && c.Blog.OwnerId == CurrentUserId);

        if (comment != null)
        {
            comment.IsApproved = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã duyệt bình luận.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.Blog)
            .FirstOrDefaultAsync(c => c.Id == id && c.Blog.OwnerId == CurrentUserId);

        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã xoá bình luận.";
        }
        return RedirectToAction("Index");
    }
}
