using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolAttendance.Models;
using SchoolAttendance.Repository;

namespace SchoolAttendance.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository SchoolRepository;

        public SchoolService(ISchoolRepository schoolRepository)
        {
            SchoolRepository = schoolRepository;
        }

        public async Task<List<SchoolClass>> GetClasses()
        {
            return await SchoolRepository.GetSchoolClasses();
        }

        public async Task<SchoolClass> GetClass(int id)
        {
            return await SchoolRepository.GetSchoolClass(id);
        }

        public async Task<List<StudentRegistration>> GetStudentsForClass(int schoolClassId, string hourOfDay)
        {
            return await SchoolRepository.GetStudentsForClass(schoolClassId, hourOfDay);
        }

        public async Task<StudentRegistration> AddStudentRegistration(StudentRegistration studentRegistration)
        {
            return await SchoolRepository.AddStudentRegistration(studentRegistration);
        }

        public async Task<Student> AddStudent(Student student)
        {
            return await SchoolRepository.AddStudent(student);
        }

        public async Task<StudentRegistration> GetStudentRegistration(int studentRegistrationId)
        {
            return await SchoolRepository.GetStudentRegistration(studentRegistrationId);
        }

        public void DeleteStudentRegistration(StudentRegistration studentRegistration)
        {
            SchoolRepository.DeleteStudentRegistration(studentRegistration);
        }

        public async Task<List<Attendance>> GetAttendancesForDay(int schoolClassId, string hourOfDay)
        {
            return await SchoolRepository.GetAttendancesForDay(schoolClassId, hourOfDay);
        }

        public async Task<bool> ScheduleNewDay()
        {
            return await SchoolRepository.ScheduleNewDay();
        }

        public async Task<bool> UpdateAttendances(List<Attendance> attendances)
        {
            return await SchoolRepository.UpdateAttendances(attendances);
        }

        public async Task<List<Attendance>> GetDailyAttendanceReport()
        {
            return await SchoolRepository.GetDailyAttendanceReport();
        }

        public List<Attendance> GetTermAttendanceReport(DateTime dateFrom, DateTime dateTo)
        {
            return SchoolRepository.GetTermAttendanceReport(dateFrom, dateTo);
        }
    }
}
