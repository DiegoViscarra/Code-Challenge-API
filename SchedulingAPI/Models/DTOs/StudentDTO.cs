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
        [StringLength(20, ErrorMessage = "First name length can't be more than 20.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Last name length can't be more than 20.")]
        public string LastName { get; set; }

        public IEnumerable<Registration> Registrations { get; set; }
    }
}
