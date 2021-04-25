using System.ComponentModel.DataAnnotations;

namespace SchoolAttendance.Entities
{
    public class StudentRegistration
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "School Class")]
        public SchoolClass SchoolClass { get; set; }
        public Student Student { get; set; }
        [Display(Name = "Hour of Day")]
        public string HourOfDay { get; set; }
    }
}
