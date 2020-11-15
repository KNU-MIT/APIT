using System.ComponentModel.DataAnnotations;
using DatabaseLayer.Enums;

namespace BusinessLayer.Models
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введіть прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введіть по-батькові")]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "Введіть мисце навчання/роботи")]
        [Display(Name = "Місце навчання/роботи")]
        public string WorkingFor { get; set; }

        [Display(Name = "Научный ступінь")] public ScienceDegree ScienceDegree { get; set; }
        // public string AltScienceDegree { get; set; }

        [Display(Name = "Академічна посада")] public AcademicTitle AcademicTitle { get; set; }
        // public string AltAcademicTitle { get; set; }

        [Display(Name = "Форма участі")] public ParticipationForm ParticipationForm { get; set; }


        [Required(ErrorMessage = "Введіть Ваш поштовий індекс")]
        [Display(Name = "Поштовий індекс")]
        public string MailboxIndex { get; set; }


        [Display(Name = "Номер контактного телефону")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Звідки Ви дізналися про конференцію?")]
        public string InfoSourceName { get; set; }


        [Compare("Password", ErrorMessage = "Введіть пароль та пидтвердіть його")]
        public string PasswordConfirm { get; set; }
    }
}