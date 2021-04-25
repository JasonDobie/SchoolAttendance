using System.ComponentModel.DataAnnotations;

namespace SchoolAttendance.Entities
{
    public class SchoolClass
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ClassName { get; set; }
        [Required]
        public string Grade { get; set; }
    }
}
