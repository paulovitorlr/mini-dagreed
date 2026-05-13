using Microsoft.EntityFrameworkCore;
using progress_service.Models;

namespace progress_service.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Progress> Progresses { get; set; }
}