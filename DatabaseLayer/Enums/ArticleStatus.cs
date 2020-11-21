using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Enums
{
    public enum ArticleStatus : short
    {
        [Display(Name = "Не підтверджено")] Uploaded = 0,
        [Display(Name = "Підтверджено")] Published = 1,

        [Display(Name = "Не підтверджено (редаговано)")]
        UploadedEdited = 3,

        [Display(Name = "Підтверджено (редаговано)")]
        PublishedEdited = 4,
        [Display(Name = "Відізвано")] Banned = 5
    }
}