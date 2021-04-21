using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolAttendance.Controllers;
using SchoolAttendance.Models;

namespace SchoolAttendance.DTO
{
    public class SelectClassDTO
    {
        public string SelectedHourOfDay { get; set; }
        public int SelectedSchoolClass { get; set; }
    }
}
