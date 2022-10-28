using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class RegistrationToClassDTO
    {
        [Required]
        public SimpleClassDTO ClassDTO { get; set; }
        [Required]
        public IEnumerable<SimpleStudentDTO> StudentsDTOs { get; set; }
    }
}
