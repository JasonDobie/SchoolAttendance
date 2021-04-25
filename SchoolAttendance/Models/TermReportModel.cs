using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAttendance.Models
{
    public class TermReportModel
    {
        [DisplayName("Class Name")]
        public string ClassName { get; set; }
        public string Grade { get; set; }
        [DisplayName("Student Name")]
        public string StudentName { get; set; }
        [DisplayName("Classes Attended")]
        public string ClassesAttended { get; set; }
        [DisplayName("Classes Missed")]
        public string ClassesMissed { get; set; }
    }
}
