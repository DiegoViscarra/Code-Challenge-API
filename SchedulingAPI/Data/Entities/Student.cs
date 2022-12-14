using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulingAPI.Data.Entities
{
    public class Student
    {
        [Key]
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string FirstName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string LastName { get; set; }
    }
}
