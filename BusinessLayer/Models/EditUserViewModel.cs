﻿using System.ComponentModel.DataAnnotations;
using DatabaseLayer.Enums;

namespace BusinessLayer.Models
{
    public class EditUserViewModel
    {
        public byte[] ProfilePhoto { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string WorkingFor { get; set; }
        public string ScienceDegree { get; set; }
        public string AcademicTitle { get; set; }
        public string ParticipationForm { get; set; }

        public string PhoneNumber { get; set; }
        
        public string ResultMessage { get; set; }
    }
}