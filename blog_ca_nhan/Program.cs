using Microsoft.EntityFrameworkCore;
using blog_ca_nhan.Data;
using blog_ca_nhan.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DbContext with Connection String from Configuration (User Secrets)
builder.Services.AddDbContext<PremiumBlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Password Hasher for User Identity
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Register Authentication
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options => {
        options.Cookie.Name = "BlogApp.Session";
        options.LoginPath = "/Account/Login";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
