using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulingAPI.Data.Entities
{
    public class Registration
    {
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [ForeignKey("Code")]
        public int Code { get; set; }
    }
}
