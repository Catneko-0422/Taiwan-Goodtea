using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Taiwan_Goodtea.api;
using Taiwan_Goodtea.Models;
using static Taiwan_Goodtea.Models.UserLoginModel;

/*
    ������]�筺���B�n�J�B���U�B�ѰO�K�X�B�n�X���\��
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
                    // �n�J���\�A�ɦV�����Ϋ��w����
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    // �ϥΪ̳Q��w�]�Ҧp�K�X���զh�����ѡ^
                    ModelState.AddModelError(string.Empty, "�b��w�Q��w�A�еy��A�աC");
                }
                else
                {
                    // �n�J���ѡ]�Ҧp�K�X���~�αb�ᤣ�s�b�^
                    ModelState.AddModelError(string.Empty, "�L�Ī��n�J���աC");
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

                    TempData["Success"] = "���U���\�I";
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
                ModelState.AddModelError(string.Empty, "�п�J�q�l�l��a�}");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // ����ܱb���O�_�s�b�A�קK���|��T
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // ���ͭ��]�K�X��Token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // �إ߭��]�K�X���s��
            var resetLink = Url.Action("ResetPassword", "Home", new { token, email }, Request.Scheme);

            // ���]�o�̬O�o�e�l�󪺦a��]�u�����Τ��ݨϥζl��A�ȵo�e�^
            System.Diagnostics.Debug.WriteLine($"���]�K�X�s���G{resetLink}");

            string subject = "���]�K�X�s��";
            string body = $"<p>���I���H�U�s�����]�K�X�G</p><a href=\"{resetLink}\">{resetLink}</a>";
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
                ModelState.AddModelError(string.Empty, "�L�Ī��K�X���]�ШD");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string email, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "�K�X�P�T�{�K�X���@�P");
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
