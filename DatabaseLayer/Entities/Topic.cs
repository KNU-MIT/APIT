﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLayer.Entities
{
    public class Topic
    {
        [Required] public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ConferenceId { get; set; }
    }
}