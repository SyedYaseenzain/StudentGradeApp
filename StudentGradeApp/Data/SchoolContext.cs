using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using StudentGradeApp.Models;

namespace StudentGradeApp.Data
{

    public class SchoolContext : DbContext
    {
        public SchoolContext()
        {
        }

        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }
}