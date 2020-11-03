using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Entities
{
    public class ConferenceParticipant
    {
        public Guid Id { get; set; }
        [Required] public string UserId { get; set; }
        public bool Subscribed { get; set; }

        [Required] public Conference Conference { get; set; }
    }

    public class ConferenceImage
    {
        public Guid Id { get; set; }
        [Required] public string ImagePath { get; set; }
        public string AltText { get; set; }

        [Required] public Conference Conference { get; set; }
    }
}