using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models
{
    public class EmailViewModel
    {
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string User { get; set; }
        public string Token { get; set; }

        [Required] public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}