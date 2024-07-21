using Microsoft.EntityFrameworkCore;
using SUAS_API.Models;

namespace SUAS_API.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options):DbContext(options)
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
