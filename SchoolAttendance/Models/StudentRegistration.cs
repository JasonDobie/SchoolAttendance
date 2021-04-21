using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAttendance.Models
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
