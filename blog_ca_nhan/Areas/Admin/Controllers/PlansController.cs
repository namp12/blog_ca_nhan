using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;

namespace blog_ca_nhan.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "CookieAuth", Policy = "PlatformAdminOnly")]
public class PlansController : Controller
{
    private readonly PremiumBlogDbContext _context;
    public PlansController(PremiumBlogDbContext context) => _context = context;

    public async Task<IActionResult> Index()
        => View(await _context.SubscriptionPlans.ToListAsync());
}
