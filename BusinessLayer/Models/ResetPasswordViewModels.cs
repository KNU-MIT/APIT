using System.ComponentModel.DataAnnotations;
using DatabaseLayer.Entities;

namespace BusinessLayer.Models
{
    public class ResetPasswordViewModel
    {
        public User User { get; set; }
        public string Token { get; set; }

        [Required(ErrorMessage = "Введіть новий пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}