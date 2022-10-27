using SchedulingAPI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class ClassDTO
    {
        [Required]
        public int Code { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public IEnumerable<Registration> Registrations { get; set; }
    }
}
