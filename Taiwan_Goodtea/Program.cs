using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taiwan_Goodtea;
using TAIWAN_GoodTea.Data;

var builder = WebApplication.CreateBuilder(args);

// 註冊資料庫連線與 Identity
builder.Services.AddDbContext<dbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<dbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 初始化資料庫與預設帳號
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await initialization.InitializeDatabaseAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"資料庫初始化失敗: {ex.Message}");
    }
}

// 設定 HTTP 請求處理流程
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
