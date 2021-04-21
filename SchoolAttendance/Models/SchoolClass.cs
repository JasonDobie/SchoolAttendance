using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAttendance.Models
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
