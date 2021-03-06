﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatabaseLayer.Enums;
using Microsoft.AspNetCore.Identity;

namespace DatabaseLayer.Entities
{
    public class User : IdentityUser
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string MiddleName { get; set; }


        [Required] public string WorkingFor { get; set; }
        public ScienceDegree ScienceDegree { get; set; }
        public AcademicTitle AcademicTitle { get; set; }

        [Required] public string MailboxIndex { get; set; }
        public string InfoSourceName { get; set; }
        public override string PhoneNumber { get; set; }

        // public string ProfilePhoto { get; set; }
        [Required] public string ProfileAddress { get; set; }


        public ICollection<UserOwnArticlesLinking> OwnArticles { get; set; }
        public ICollection<ConferenceParticipant> OwnParticipation { get; set; }


        public User()
        {
            OwnArticles = new HashSet<UserOwnArticlesLinking>();
            OwnParticipation = new HashSet<ConferenceParticipant>();
        }


        [NotMapped] public string FullName => $"{LastName} {FirstName} {MiddleName}";
        public static bool operator ==(User a, User b) => a?.Id == b?.Id;
        public static bool operator !=(User a, User b) => !(a == b);

        public override int GetHashCode() => Id.GetHashCode();

        private bool Equals(User other) => other != null && Id == other.Id;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((User) obj);
        }

        public override string ToString() =>
            $"{LastName} {FirstName} {MiddleName} -> " +
            $"email({EmailConfirmed}):{Email} " +
            $"phone({PhoneNumberConfirmed}):{PhoneNumber}";
    }
}