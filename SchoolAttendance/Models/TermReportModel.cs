using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAttendance.Models
{
    public class TermReportModel
    {
        public string ClassName { get; set; }
        public string Grade { get; set; }
        public string StudentName { get; set; }
        public string ClassesAttended { get; set; }
        public string ClassesMissed { get; set; }
    }
}
