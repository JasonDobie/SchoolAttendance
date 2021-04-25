using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolAttendance.Entities
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public StudentRegistration StudentRegistration { get; set; }
        [Required]
        public DateTime ClassDate { get; set; }
        public bool Attended { get; set; }
    }
}
