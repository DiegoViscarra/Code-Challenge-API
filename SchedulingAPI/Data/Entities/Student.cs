using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Data.Entities
{
    public class Student
    {
        [Key]
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public ICollection<Registration> Registrations { get; set; }
    }
}
