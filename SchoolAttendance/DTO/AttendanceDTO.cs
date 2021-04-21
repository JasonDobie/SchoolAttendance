using System.Collections.Generic;
using SchoolAttendance.Models;

namespace SchoolAttendance.DTO
{
    public class AttendanceDTO
    {
        public int Id { get; set; }
        public List<AttendanceItem> Attendances { get; set; }
    }
}
