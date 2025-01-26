using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Taiwan_Goodtea;
using TAIWAN_GoodTea.Data;

var builder = WebApplication.CreateBuilder(args);

// ���U EmailService
builder.Services.AddTransient<Taiwan_Goodtea.api.EmailService>();

// ���U��Ʈw�s�u�P Identity
builder.Services.AddDbContext<dbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // �K�X�����]�w
    options.Password.RequireDigit = true;             // �����]�t�Ʀr
    options.Password.RequiredLength = 8;              // �̤p����
    options.Password.RequireNonAlphanumeric = false;  // �����]�t�D�r���Ʀr�r��
    options.Password.RequireUppercase = true;         // �����]�t�j�g�r��
    options.Password.RequireLowercase = true;         // �����]�t�p�g�r��
    options.Password.RequiredUniqueChars = 1;         // �����]�t�ܤ� 1 �Ӱߤ@�r��
})
.AddEntityFrameworkStores<dbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ��l�Ƹ�Ʈw�P�w�]�b��
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await initialization.InitializeDatabaseAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"��Ʈw��l�ƥ���: {ex.Message}");
    }
}

// �]�w HTTP �ШD�B�z�y�{
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
