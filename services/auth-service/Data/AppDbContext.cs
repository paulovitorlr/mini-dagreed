using Microsoft.EntityFrameworkCore;
using auth_service.Models;

namespace auth_service.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users {  get; set; }
    }
}
