using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolAttendance.Data;
using SchoolAttendance.Entities;
using SchoolAttendance.Models;

namespace SchoolAttendance.Repository
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly SchoolDbContext SchoolDbContext;

        public SchoolRepository(SchoolDbContext schoolDbContext)
        {
            SchoolDbContext = schoolDbContext;
        }

        public Task<List<SchoolClass>> GetSchoolClasses()
        {
            try
            {
                return SchoolDbContext.Set<SchoolClass>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve classes: {ex.Message}");
            }
        }

        public Task<SchoolClass> GetSchoolClass(int id)
        {
            try
            {
                return SchoolDbContext.Set<SchoolClass>().FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve class: {ex.Message}");
            }
        }

        public async Task<List<StudentRegistration>> GetStudentsForClass(int schoolClassId, string hourOfDay)
        {
            try
            {
                return await SchoolDbContext.Set<StudentRegistration>()
                    .Include(sr => sr.Student)
                    .Where(sr => sr.SchoolClass.Id == schoolClassId
                            && sr.HourOfDay == hourOfDay).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve students: {ex.Message}");
            }
        }

        public async Task<Student> AddStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException($"Student must not be null");
            }

            try
            {
                await SchoolDbContext.AddAsync(student);
                await SchoolDbContext.SaveChangesAsync();

                return student;
            }
            catch (Exception ex)
            {
                throw new Exception($"Student could not be saved: {ex.Message}");
            }
        }

        public async Task<StudentRegistration> AddStudentRegistration(StudentRegistration studentRegistration)
        {
            if (studentRegistration == null)
            {
                throw new ArgumentNullException($"Student registration must not be null");
            }
            
            try
            {
                await SchoolDbContext.AddAsync(studentRegistration);
                await SchoolDbContext.SaveChangesAsync();

                var attendances = SchoolDbContext.Set<Attendance>().FirstOrDefault(a => a.ClassDate.Date == DateTime.Now.Date);
                if (attendances != null)
                {
                    // Attendances have already been scheduled for today. Create attendance for new student
                    Attendance attendance = new Attendance();
                    attendance.ClassDate = DateTime.Now;
                    attendance.StudentRegistration = studentRegistration;
                    attendance.Attended = false;

                    await SchoolDbContext.AddAsync(attendance);
                    await SchoolDbContext.SaveChangesAsync();
                }

                return studentRegistration;
            }
            catch (Exception ex)
            {
                throw new Exception($"Student registration could not be saved: {ex.Message}");
            }
        }

        public async Task<StudentRegistration> GetStudentRegistration(int studentRegistrationId)
        {
            try
            {
                return await SchoolDbContext.Set<StudentRegistration>()
                    .Include(sr => sr.Student)
                    .Include(sr => sr.SchoolClass)
                    .FirstOrDefaultAsync(sr => sr.Id == studentRegistrationId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve student registration: {ex.Message}");
            }
        }

        public void DeleteStudentRegistration(StudentRegistration studentRegistration)
        {
            if (studentRegistration == null)
            {
                throw new ArgumentNullException($"Student registration must not be null");
            }

            try
            {
                var attendances = SchoolDbContext.Set<Attendance>()
                    .Include(a => a.StudentRegistration)
                    .Where(a => a.StudentRegistration.Id == studentRegistration.Id);
                
                foreach (var attendance in attendances)
                {
                    SchoolDbContext.Set<Attendance>().Remove(attendance);
                }

                SchoolDbContext.Students.Remove(studentRegistration.Student);
                SchoolDbContext.StudentRegistrations.Remove(studentRegistration);

                SchoolDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Student registration could not be deleted: {ex.Message}");
            }
        }

        public async Task<List<Attendance>> GetAttendancesForDay(int schoolClassId, string hourOfDay)
        {
            try
            {   
                return await SchoolDbContext.Set<Attendance>()
                    .Include(a => a.StudentRegistration)
                    .ThenInclude(sr => sr.Student)
                    .Where(a => a.StudentRegistration.SchoolClass.Id == schoolClassId && 
                                a.StudentRegistration.HourOfDay == hourOfDay).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve student attendances: {ex.Message}");
            }
        }

        public async Task<bool> ScheduleNewDay()
        {
            try
            {
                if (SchoolDbContext.Set<Student>().Count() == 0)
                    return false;

                var existingAttendances = SchoolDbContext.Set<Attendance>().Any(a => a.ClassDate.Date == DateTime.Now.Date);
                if (existingAttendances == true)
                    return true;

                var classes = await GetSchoolClasses();
                foreach (var schoolClass in classes)
                {
                    await ScheduleAttendances(schoolClass, "8 am");
                    await ScheduleAttendances(schoolClass, "9 am");
                    await ScheduleAttendances(schoolClass, "10 am");
                    await ScheduleAttendances(schoolClass, "11 am");
                    await ScheduleAttendances(schoolClass, "1 pm");
                    await ScheduleAttendances(schoolClass, "2 pm");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't save new day: {ex.Message}");
            }
        }

        private async Task<bool> ScheduleAttendances(SchoolClass schoolClass, string hourOfDay)
        {
            DateTime today = System.DateTime.Now;
            Attendance attendance = null;

            try
            {
                var students = await GetStudentsForClass(schoolClass.Id, hourOfDay);
                foreach (var student in students)
                {
                    attendance = new Attendance();
                    attendance.ClassDate = today;
                    attendance.StudentRegistration = student;
                    attendance.Attended = false;

                    await SchoolDbContext.AddAsync(attendance);
                    await SchoolDbContext.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't schedule attendances: {ex.Message}");
            }
        }

        public async Task<bool> UpdateAttendances(AttendanceModel attendancesResult, int schoolClassId, string hourOfDay)
        {
            List<Attendance> attendances = await GetAttendancesForDay(schoolClassId, hourOfDay);

            foreach (Attendance attendance in attendances)
            {
                foreach (AttendanceItem item in attendancesResult.Attendances)
                {
                    if (item.Id == attendance.Id)
                    {
                        attendance.Attended = item.Attended;
                        SchoolDbContext.Update(attendance);
                        break;
                    }
                }
            }
            await SchoolDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Attendance>> GetDailyAttendanceReport()
        {
            try
            {
                return await SchoolDbContext.Set<Attendance>()
                    .Include(a => a.StudentRegistration.SchoolClass)
                    .Include(a => a.StudentRegistration)
                    .ThenInclude(sr => sr.Student)
                    .OrderBy(a => a.StudentRegistration.SchoolClass)
                    .ThenBy(a => a.StudentRegistration.Student.LastName)
                    .ThenBy(a => a.StudentRegistration.Student.FirstName)
                    .Where(a => a.ClassDate.Date == System.DateTime.Now.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve daily report: {ex.Message}");
            }
        }

        public List<TermReportModel> GetTermAttendanceReport(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                dateTo = dateTo.AddDays(1); // Ensure today's date is included
                string sql = $@"
                    select ClassName, Grade, FirstName + " + new string("' '") + $@" + LastName,
	                    (select count(*) from Attendances a1 inner join StudentRegistrations sr2 on sr2.Id = a1.StudentRegistrationId 
		                    where sr2.StudentId = s.Id and a1.Attended = 1),
	                    (select count(*) from Attendances a1 inner join StudentRegistrations sr2 on sr2.Id = a1.StudentRegistrationId 
		                    where sr2.StudentId = s.Id and a1.Attended = 0), a.classdate
                    from Students s
                    inner join StudentRegistrations sr on sr.StudentId = s.Id
                    inner join Classes c on c.Id = sr.SchoolClassId
                    inner join Attendances a on a.StudentRegistrationId = sr.Id
                    where a.ClassDate >= '{dateFrom.Date}' and a.ClassDate <= '{dateTo.Date}'
                    group by ClassName, Grade, FirstName, LastName, StudentRegistrationId, s.Id, a.classdate";
                    
                List<TermReportModel> attendances = new List<TermReportModel>();

                using (var command = SchoolDbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    SchoolDbContext.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            attendances.Add(new TermReportModel { ClassName = result[0].ToString(), Grade = result[1].ToString(),
                                StudentName = result[2].ToString(), ClassesAttended = result[3].ToString(), ClassesMissed = result[4].ToString() });
                        }
                    }
                }

                return attendances;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve term report: {ex.Message}");
            }
        }
    }
}
