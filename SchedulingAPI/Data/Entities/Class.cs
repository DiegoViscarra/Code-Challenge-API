﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulingAPI.Data.Entities
{
    public class Class
    {
        [Key]
        [Required]
        public int Code { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string Description { get; set; }

        public ICollection<Registration> Registrations { get; set; }
    }
}
