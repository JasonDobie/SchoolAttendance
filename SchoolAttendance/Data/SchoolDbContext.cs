using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolAttendance.Entities;
using SchoolAttendance.Models;

namespace SchoolAttendance.Data
{
    public class SchoolDbContext : IdentityDbContext<IdentityUser>
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<SchoolClass> Classes { get; set; }
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
