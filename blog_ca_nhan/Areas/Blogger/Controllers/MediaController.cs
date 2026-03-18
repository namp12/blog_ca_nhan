using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;
using System.Security.Claims;

namespace blog_ca_nhan.Areas.Blogger.Controllers;

[Area("Blogger")]
[Authorize(AuthenticationSchemes = "CookieAuth")]
public class MediaController : Controller
{
    private readonly PremiumBlogDbContext _context;
    private readonly IWebHostEnvironment _environment;

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public MediaController(PremiumBlogDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ActiveMenu"] = "media";
        var media = await _context.Media
            .Where(m => m.UserId == CurrentUserId)
            .OrderByDescending(m => m.UploadedAt)
            .ToListAsync();
        return View(media);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, string? title)
    {
        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Vui lòng chọn một tệp tin.";
            return RedirectToAction("Index");
        }

        // Create uploads folder if not exists
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "media", CurrentUserId.ToString());
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var media = new Medium
        {
            UserId = CurrentUserId,
            FileName = file.FileName,
            FilePath = $"/uploads/media/{CurrentUserId}/{uniqueFileName}",
            FileType = file.ContentType,
            FileSize = file.Length,
            UploadedAt = DateTime.Now
        };

        _context.Media.Add(media);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Đã tải lên tệp tin thành công!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var media = await _context.Media.FirstOrDefaultAsync(m => m.Id == id && m.UserId == CurrentUserId);
        if (media != null)
        {
            // Delete physical file
            var filePath = Path.Combine(_environment.WebRootPath, media.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã xoá tệp tin.";
        }
        return RedirectToAction("Index");
    }
}
