using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Enums
{
    public enum AcademicTitle : short
    {
        [Display(Name = "Академічна посада")] Other = 0,
        [Display(Name = "Доцент")] AssistantProfessor = 1,
        [Display(Name = "Професор")] Professor = 2,
    }
}