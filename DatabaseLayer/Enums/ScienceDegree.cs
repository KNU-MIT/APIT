using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Enums
{
    public enum ScienceDegree : short
    {
        [Display(Name = "Науковий ступінь")] Other = 0,
        [Display(Name = "Бакалавр")] Bachelor = 1,
        [Display(Name = "Магістр")] Master,
        [Display(Name = "Аспірант")] Graduate,
        [Display(Name = "Кандидат наук")] Candidate,
        [Display(Name = "Доктор наук")] Doctor,
    }
}