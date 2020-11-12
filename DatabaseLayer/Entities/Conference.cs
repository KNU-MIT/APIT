using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DatabaseLayer.Entities
{
    public class Conference
    {
        public Guid Id { get; set; }

        [Required] public string UniqueAddress { get; set; }

        public bool IsActual { get; set; }
        // public bool IsRemote { get; set; }

        [Required] public string Title { get; set; }

        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public ICollection<ConferenceParticipant> Participants { get; set; }
        public ICollection<ConferenceImage> Images { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<Topic> Topics { get; set; }
        public ICollection<ConferenceDate> Dates { get; set; }

        [DataType(DataType.Date)] public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)] public DateTime DateLastModified { get; set; }
        [DataType(DataType.DateTime)] public DateTime DateStart { get; set; }
        [DataType(DataType.DateTime)] public DateTime DateFinish { get; set; }


        public Conference()
        {
            Participants = new HashSet<ConferenceParticipant>();
            Articles = new HashSet<Article>();
            Images = new HashSet<ConferenceImage>();
            Topics = new HashSet<Topic>();
            Dates = new HashSet<ConferenceDate>();
        }


        public static bool operator ==(Conference a, Conference b) => a?.Id == b?.Id;
        public static bool operator !=(Conference a, Conference b) => !(a == b);

        public override int GetHashCode() => Id.GetHashCode();

        private bool Equals(Conference other) => other != null && Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Conference) obj);
        }
    }
}