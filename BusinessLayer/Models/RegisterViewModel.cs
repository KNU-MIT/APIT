using System.ComponentModel.DataAnnotations;
using DatabaseLayer;
using DatabaseLayer.Enums;

namespace BusinessLayer.Models
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я")] public string FirstName { get; set; }

        [Required(ErrorMessage = "Введіть прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введіть по-батькові")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "Введіть мисце навчання/роботи")]
        [Display(Name = "Місце навчання/роботи")]
        public string WorkingFor { get; set; }

        [Display(Name = "Научна ступінь")] public ScienceDegree ScienceDegree { get; set; }
        public string AltScienceDegree { get; set; }


        [Display(Name = "Академічна посада")] public AcademicTitle AcademicTitle { get; set; }
        public string AltAcademicTitle { get; set; }

        [Display(Name = "Форма участі")] public ParticipationForm ParticipationForm { get; set; }


        [Required(ErrorMessage = "Введіть пароль та пидтвердіть його")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}