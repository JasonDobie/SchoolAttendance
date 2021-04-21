using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolAttendance.Data;
using SchoolAttendance.DTO;
using SchoolAttendance.Models;
using SchoolAttendance.Services;

namespace SchoolAttendance.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class SchoolController : Controller
    {
        private readonly ISchoolService SchoolService;

        public SchoolController(ISchoolService schoolService)
        {
            SchoolService = schoolService;
        }

        public async Task<IActionResult> SelectClass(string registerAttend)
        {
            ViewBag.Classes = await SchoolService.GetClasses();

            List<HourOfDayList> hoursOfDay = new List<HourOfDayList> { 
                new HourOfDayList { HourOfDay = "8 am", HourOfDayText = "8 am" }, 
                new HourOfDayList { HourOfDay = "9 am", HourOfDayText = "9 am" }, 
                new HourOfDayList { HourOfDay = "10 am", HourOfDayText = "10 am" }, 
                new HourOfDayList { HourOfDay = "11 am", HourOfDayText = "11 am" },
                new HourOfDayList { HourOfDay = "1 pm", HourOfDayText = "1 pm" },
                new HourOfDayList { HourOfDay = "2 pm", HourOfDayText = "2 pm" },
                };
            ViewBag.HoursOfDay = hoursOfDay;
            ViewBag.RegisterAttend = registerAttend;

            return View(new SelectClassDTO());
        }

        [HttpPost]
        public IActionResult SelectClass(SelectClassDTO selectClass)
        {
            if (Request.Form["RegisterAttend"] == "Register")
                return RedirectToAction("RegisterStudents", new { schoolClassId = selectClass.SelectedSchoolClass, hourOfDay = selectClass.SelectedHourOfDay });
            else
                return RedirectToAction("StudentAttendance", new { schoolClassId = selectClass.SelectedSchoolClass, hourOfDay = selectClass.SelectedHourOfDay });
        }

        public async Task<IActionResult> RegisterStudents(int schoolClassId, string hourOfDay)
        {
            var schoolClass = await SchoolService.GetClass(schoolClassId);
            ViewBag.SchoolClass = schoolClass.ClassName;
            ViewBag.SchoolClassId = schoolClass.Id;

            ViewBag.HourOfDay = hourOfDay;

            List<StudentRegistration> studentRegistrations = await SchoolService.GetStudentsForClass(schoolClassId, hourOfDay);

            return View(studentRegistrations);
        }

        public IActionResult AddStudent(int schoolClassId, string hourOfDay)
        {
            ViewBag.SchoolClassId = schoolClassId;
            ViewBag.HourOfDay = hourOfDay;

            var student = new Student();

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            int schoolClassId = Convert.ToInt32(Request.Form["schoolClassId"]);
            string hourOfDay = Request.Form["hourOfDay"];

            StudentRegistration studentRegistrationNew = new StudentRegistration();

            if (ModelState.IsValid)
            {
                var studentNew = await SchoolService.AddStudent(student);

                var studentRegistration = new StudentRegistration();
                
                var schoolClass = await SchoolService.GetClass(schoolClassId);
                studentRegistration.SchoolClass = schoolClass;

                studentRegistration.Student = studentNew;
                studentRegistration.HourOfDay = hourOfDay;

                studentRegistrationNew = await SchoolService.AddStudentRegistration(studentRegistration);
            }

            return RedirectToAction("AddStudent", new { schoolClassId = schoolClassId, hourOfDay = hourOfDay });
        }

        public async Task<IActionResult> DeleteStudentRegistration(int Id, int schoolClassId, string hourOfDay)
        {
            var studentRegistration = await SchoolService.GetStudentRegistration(Id);
            SchoolService.DeleteStudentRegistration(studentRegistration);

            return RedirectToAction("RegisterStudents", new { schoolClassId = schoolClassId, hourOfDay = hourOfDay });
        }

        public async Task<IActionResult> ScheduleNewDay()
        {
            await SchoolService.ScheduleNewDay();

            ViewBag.Success = true;

            return RedirectToAction("Index", "Home", new { success = true } );
        }

        [HttpGet]
        public async Task<IActionResult> StudentAttendance(int schoolClassId, string hourOfDay)
        {
            /*ViewBag.Classes = await SchoolService.GetClasses();

            List<HourOfDayList> hoursOfDay = new List<HourOfDayList> {
                new HourOfDayList { HourOfDay = "8 am", HourOfDayText = "8 am" },
                new HourOfDayList { HourOfDay = "9 am", HourOfDayText = "9 am" },
                new HourOfDayList { HourOfDay = "10 am", HourOfDayText = "10 am" },
                new HourOfDayList { HourOfDay = "11 am", HourOfDayText = "11 am" },
                new HourOfDayList { HourOfDay = "1 pm", HourOfDayText = "1 pm" },
                new HourOfDayList { HourOfDay = "2 pm", HourOfDayText = "2 pm" },
                };
            ViewBag.HoursOfDay = hoursOfDay;

            return View(new SelectClassDTO());*/

            ViewBag.RegisterAttend = "Attend";

            List<Attendance> attendances = await SchoolService.GetAttendancesForDay(schoolClassId, hourOfDay);

            AttendanceDTO attendanceDTO = new AttendanceDTO();
            attendanceDTO.Attendances = attendances.Select(a => new AttendanceItem()
            {
                Id = a.Id,
                FirstName = a.StudentRegistration.Student.FirstName,
                LastName = a.StudentRegistration.Student.LastName,
                Attended = a.Attended
            }).ToList();

            //List<AttendanceDTO> attendanceDTOs = new List<AttendanceDTO>();
            //foreach (var attendance in attendances)
            //{
            //    attendanceDTOs.Add(new AttendanceDTO { Id = attendance.Id,
            //                                           FirstName = attendance.StudentRegistration.Student.FirstName,
            //                                           LastName = attendance.StudentRegistration.Student.LastName,
            //                                           Attended = attendance.Attended });
            //}

            return View(attendanceDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StudentAttendance(AttendanceDTO attendances)
        {
            //await SchoolService.UpdateAttendances(attendances);
            //foreach (var item in attendances.Attendances)
            //{

            //}
            

            return RedirectToAction("Index", "Home");
        }


        public bool SaveAttendances(IEnumerable<AttendanceDTO> attendances)
        {
            return true;
        }

        public async Task<IActionResult> DailyAttendanceReport()
        {
            var attendances = await SchoolService.GetDailyAttendanceReport();

            return View(attendances);    
        }

        [HttpGet]
        public IActionResult TermReportDates()
        {
            var termDates = new TermReportDTO();

            return View(termDates);
        }

        [HttpPost]
        public IActionResult TermReportDates(TermReportDTO termReportDates)
        {
            var termDates = new TermReportDTO();

            string datefrom = Request.Form["datefrom"].ToString();
            string dateto = Request.Form["dateto"].ToString();

            return RedirectToAction("TermAttendanceReport", new { dateFrom = Convert.ToDateTime(datefrom), dateTo = Convert.ToDateTime(dateto) });
        }

        public IActionResult TermAttendanceReport(DateTime dateFrom, DateTime dateTo)
        {
            List<Attendance> attendances = SchoolService.GetTermAttendanceReport(dateFrom, dateTo);

            return View(attendances);
        }
    }

    public class HourOfDayList
    {
        public string HourOfDay { get; set; }
        public string HourOfDayText { get; set; }
    }
}
