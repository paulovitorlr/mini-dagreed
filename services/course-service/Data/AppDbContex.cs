using Microsoft.EntityFrameworkCore;
using course_service.Models;

namespace course_service.Data;

    public class AppDbContext : DbContext
    {
    
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Course> Courses { get; set; }

    }

