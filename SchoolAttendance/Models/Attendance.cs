using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAttendance.Models
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
