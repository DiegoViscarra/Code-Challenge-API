using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class SimpleClassDTO
    {
        public Guid Code { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Title length can't be more than 30.")]
        public string Title { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Description length can't be more than 250.")]
        public string Description { get; set; }
    }
}
