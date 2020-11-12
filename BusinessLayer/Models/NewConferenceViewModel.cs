using System;
using System.ComponentModel.DataAnnotations;
using DatabaseLayer;

namespace BusinessLayer.Models
{
    // Why don't we use the ConferenceViewModel for represent the new instance of the Conference?
    // ANSWER: because there are a lot of unnecessary redundant fields
    public class NewConferenceViewModel
    {
        [Display(Name = MSG.UniqueAddressName)]
        public string UniqueAddress { get; set; }


        [Required(ErrorMessage = "Вкажіть назву конференції")]
        [Display(Name = "Назва конференції")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Введіть короткий опис")]
        [Display(Name = "Короткий опис")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Введить повний опис (допускається HTML)")]
        [Display(Name = "Повний опис")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Вкажіть як мінімум одну тему")]
        [Display(Name = "Теми конференції")]
        public string[] Topics { get; set; }


        [Required(ErrorMessage = MSG.OnRequired)]
        [DataType(DataType.DateTime)]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = MSG.OnRequired)]
        [DataType(DataType.DateTime)]
        public DateTime DateFinish { get; set; }
    }
}