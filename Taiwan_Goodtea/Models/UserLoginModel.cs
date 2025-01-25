using System.ComponentModel.DataAnnotations;

namespace Taiwan_Goodtea.Models
{
    public class UserLoginModel
    {
        public class LoginViewModel
        {
            [Required(ErrorMessage = "請輸入使用者名稱")]
            [Display(Name = "使用者名稱")]
            public string Username { get; set; }

            [Required(ErrorMessage = "請輸入密碼")]
            [DataType(DataType.Password)]
            [Display(Name = "密碼")]
            public string Password { get; set; }

            [Display(Name = "記住我")]
            public bool RememberMe { get; set; }
        }

        public class RegisterViewModel
        {
            [Required(ErrorMessage = "請輸入使用者名稱")]
            [Display(Name = "使用者名稱")]
            public string Username { get; set; }

            [Required(ErrorMessage = "請輸入電子郵件")]
            [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
            [Display(Name = "電子郵件")]
            public string Email { get; set; }

            [Required(ErrorMessage = "請輸入密碼")]
            [DataType(DataType.Password)]
            [Display(Name = "密碼")]
            public string Password { get; set; }

            [Required(ErrorMessage = "請確認密碼")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "密碼與確認密碼不一致")]
            [Display(Name = "確認密碼")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "請選擇角色")]
            [Display(Name = "角色")]
            public string Role { get; set; }
        }
    }
}
