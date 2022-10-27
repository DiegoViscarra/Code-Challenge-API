using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Data.Entities
{
    public class Class
    {
        [Key]
        [Required]
        public int Code { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public ICollection<Registration> Registrations { get; set; }
    }
}
