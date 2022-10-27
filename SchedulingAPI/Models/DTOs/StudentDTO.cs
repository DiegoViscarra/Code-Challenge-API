using SchedulingAPI.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class StudentDTO
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public IEnumerable<Registration> Registrations { get; set; }
    }
}
