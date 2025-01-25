using Microsoft.AspNetCore.Identity;

namespace Taiwan_Goodtea
{
    public static class initialization
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // 檢查並創建 "Admin" 角色
            string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // 檢查並創建預設的管理員帳號
            string adminUsername = "Admin123";
            string adminEmail = "Admin123@gmail.com";
            string adminPassword = "Adminpassword_123";

            if (await userManager.FindByNameAsync(adminUsername) == null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminUsername,
                    Email = adminEmail,
                    EmailConfirmed = true // 可以設定 Email 為已驗證
                };

                var createUserResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    Console.WriteLine("管理員帳號已成功建立");
                }
                else
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        Console.WriteLine($"錯誤: {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("管理員帳號已存在");
            }
        }
    }
}
