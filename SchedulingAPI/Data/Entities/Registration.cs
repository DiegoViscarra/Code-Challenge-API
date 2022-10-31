using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulingAPI.Data.Entities
{
    public class Registration
    {
        [ForeignKey("StudentId")]
        public Guid StudentId { get; set; }

        [ForeignKey("Code")]
        public Guid Code { get; set; }
    }
}
