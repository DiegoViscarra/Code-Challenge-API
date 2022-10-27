using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulingAPI.Data.Entities
{
    public class Registration
    {
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Code")]
        public int Code { get; set; }
        public Class Class { get; set; }
    }
}
