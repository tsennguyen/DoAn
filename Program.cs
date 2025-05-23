using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping_Laptop.Models;
using Shopping_Laptop.Models.Momo;
using Shopping_Laptop.Repository;
using Shopping_Laptop.Services.Momo;
using Shopping_Laptop.Services.Recommendation;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình MOMO API
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();

// Cấu hình đăng nhập Google
builder.Services.AddAuthentication()
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        var clientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
        var clientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new InvalidOperationException("Google authentication credentials are not configured.");
        }

        options.ClientId = clientId;
        options.ClientSecret = clientSecret;
    });

// Thêm các services
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

// Cấu hình DbContext
var connectionString = builder.Configuration.GetConnectionString("Shopping_LaptopContext");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

// Thêm Email Sender
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Thêm RecommendationService
builder.Services.AddScoped<IRecommendationService, RecommendationService>();

// Cấu hình Identity
builder.Services.AddIdentity<AppUserModel, IdentityRole>(options =>
{
    // Cấu hình mật khẩu
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Cấu hình user
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Định tuyến
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

app.MapControllerRoute(
   name: "category",
   pattern: "category/{slug?}",
   defaults: new { controller = "Category", action = "Index" });

app.MapControllerRoute(
   name: "brand",
   pattern: "brand/{slug?}",
   defaults: new { controller = "Brand", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();