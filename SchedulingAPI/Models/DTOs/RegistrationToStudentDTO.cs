using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class RegistrationToStudentDTO
    {
        [Required]
        public IEnumerable<SimpleClassDTO> SimpleClassesDTOs { get; set; }
        [Required]
        public SimpleStudentDTO SimpleStudentDTO { get; set; }
    }
}
