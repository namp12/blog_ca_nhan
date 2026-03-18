using blog_ca_nhan.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Data;

public static class DbInitializer
{
    public static async Task SeedAdminUser(IServiceProvider serviceProvider)
    {
        using var context = new PremiumBlogDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<PremiumBlogDbContext>>());

        // 1. Kiểm tra xem đã có User nào chưa (hoặc có admin chưa)
        if (await context.Users.AnyAsync(u => u.Username == "admin"))
        {
            return; // Đã có admin, không làm gì cả
        }

        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<User>>();

        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@premium-blog.local",
            DisplayName = "Platform Administrator",
            CreatedAt = DateTime.Now,
            IsActive = true,
            IsPlatformAdmin = true
        };

        // Băm mật khẩu mặc định: Admin@123
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123");

        context.Users.Add(adminUser);

        // 2. Thêm một số dữ liệu mẫu khác nếu cần (ví dụ: Themes, Plans)
        if (!await context.SubscriptionPlans.AnyAsync())
        {
            context.SubscriptionPlans.AddRange(
                new SubscriptionPlan { Name = "Free", Price = 0, MaxPosts = 10, MaxStorageMb = 100, HasCustomDomain = false, HasAdFree = false },
                new SubscriptionPlan { Name = "Premium", Price = 99, MaxPosts = -1, MaxStorageMb = 5120, HasCustomDomain = true, HasAdFree = true }
            );
        }

        if (!await context.Themes.AnyAsync())
        {
            context.Themes.AddRange(
                new Theme { Name = "Minimalist", CodeName = "minimalist", Description = "Clean and simple design for writers.", IsActive = true, IsPremium = false },
            new Theme { Name = "Modern", CodeName = "modern", Description = "Grid layout with sleek animations.", IsActive = true, IsPremium = false },
            new Theme { Name = "E-Commerce", CodeName = "e-commerce", Description = "Perfect for small shops and boutiques.", IsActive = true, IsPremium = true }
         );
        }

        await context.SaveChangesAsync();
    }
}
