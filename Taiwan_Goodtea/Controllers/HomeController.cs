using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Taiwan_Goodtea.api;
using Taiwan_Goodtea.Models;
using static Taiwan_Goodtea.Models.UserLoginModel;

/*
    此控制器包函首頁、登入、註冊、忘記密碼、登出等功能
*/

namespace Taiwan_Goodtea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, EmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =  await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // 登入成功，導向首頁或指定頁面
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    // 使用者被鎖定（例如密碼嘗試多次失敗）
                    ModelState.AddModelError(string.Empty, "帳戶已被鎖定，請稍後再試。");
                }
                else
                {
                    // 登入失敗（例如密碼錯誤或帳戶不存在）
                    ModelState.AddModelError(string.Empty, "無效的登入嘗試。");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    await _userManager.AddToRoleAsync(user, model.Role);

                    TempData["Success"] = "註冊成功！";
                    return RedirectToAction("Home", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "請輸入電子郵件地址");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // 不顯示帳號是否存在，避免洩漏資訊
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // 產生重設密碼的Token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // 建立重設密碼的連結
            var resetLink = Url.Action("ResetPassword", "Home", new { token, email }, Request.Scheme);

            // 假設這裡是發送郵件的地方（真實應用中需使用郵件服務發送）
            System.Diagnostics.Debug.WriteLine($"重設密碼連結：{resetLink}");

            string subject = "重設密碼連結";
            string body = $"<p>請點擊以下連結重設密碼：</p><a href=\"{resetLink}\">{resetLink}</a>";
            await _emailService.SendEmailAsync(email, subject, body);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "無效的密碼重設請求");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "密碼與確認密碼不一致");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
