using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolAttendance.Models;

namespace SchoolAttendance.Services
{
    public interface ISchoolService
    {
        public Task<List<SchoolClass>> GetClasses();
        public Task<SchoolClass> GetClass(int id);
        public Task<List<StudentRegistration>> GetStudentsForClass(int schoolClassId, string hourOfDay);
        public Task<StudentRegistration> AddStudentRegistration(StudentRegistration studentRegistration);
        public Task<Student> AddStudent(Student student);
        public Task<StudentRegistration> GetStudentRegistration(int studentRegistrationId);
        public void DeleteStudentRegistration(StudentRegistration studentRegistration);
        public Task<List<Attendance>> GetAttendancesForDay(int schoolClassId, string hourOfDay);
        public Task<bool> ScheduleNewDay();
        public Task<bool> UpdateAttendances(List<Attendance> attendances);
        public Task<List<Attendance>> GetDailyAttendanceReport();
        public List<Attendance> GetTermAttendanceReport(DateTime dateFrom, DateTime dateTo);
    }
}
