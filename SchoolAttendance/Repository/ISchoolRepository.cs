using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolAttendance.Entities;
using SchoolAttendance.Models;

namespace SchoolAttendance.Repository
{
    public interface ISchoolRepository 
    {
        public Task<List<SchoolClass>> GetSchoolClasses();
        public Task<SchoolClass> GetSchoolClass(int id);
        public Task<List<StudentRegistration>> GetStudentsForClass(int schoolClassId, string hourOfDay);
        public Task<StudentRegistration> AddStudentRegistration(StudentRegistration studentRegistration);
        public Task<StudentRegistration> GetStudentRegistration(int studentRegistrationId);
        public Task<Student> AddStudent(Student student);
        public void DeleteStudentRegistration(StudentRegistration studentRegistration);
        public Task<List<Attendance>> GetAttendancesForDay(int schoolClassId, string hourOfDay);
        public Task<bool> ScheduleNewDay();
        public Task<bool> UpdateAttendances(AttendanceModel attendances, int schoolClassId, string hourOfDay);
        public Task<List<Attendance>> GetDailyAttendanceReport();
        public List<TermReportModel> GetTermAttendanceReport(DateTime dateFrom, DateTime dateTo);
    }
}
