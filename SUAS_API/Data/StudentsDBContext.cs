using Microsoft.EntityFrameworkCore;
using SUAS_API.Models;

namespace SUAS_API.Data
{
    public class StudentsDBContext(DbContextOptions<StudentsDBContext> options):DbContext(options)
    {
        public DbSet<Student> Students { get; set; }
    }
}
