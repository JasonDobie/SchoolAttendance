using System.ComponentModel.DataAnnotations;

namespace SchoolAttendance.Entities
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }
    }
}
