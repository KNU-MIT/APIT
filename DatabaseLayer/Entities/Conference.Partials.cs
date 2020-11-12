using System;
using System.ComponentModel.DataAnnotations;
using DatabaseLayer.Enums;

namespace DatabaseLayer.Entities
{
    public class ConferenceParticipant
    {
        public Guid Id { get; set; }

        public ParticipationForm ParticipationForm { get; set; }
        public string AdditionalConditions { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        [Required] public Conference Conference { get; set; }
    }

    public class ConferenceImage
    {
        public Guid Id { get; set; }
        [Required] public string ImagePath { get; set; }
        public string AltText { get; set; }

        [Required] public Conference Conference { get; set; }
    }

    public class ConferenceDate
    {
        public Guid Id { get; set; }

        [Required] public string Name { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required] public Conference Conference { get; set; }
    }
}