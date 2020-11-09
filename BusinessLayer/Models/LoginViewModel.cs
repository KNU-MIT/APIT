using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть Вашу пошту")]
        // [UIHint("email")]
        [Display(Name = "Email")] // It is better to use the Localization and the Resource files.
        public string Email { get; set; }

        // [UIHint("password")]
        [Display(Name = "Пароль")] // It is better to use the Localization and the Resource files.
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}