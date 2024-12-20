using Microsoft.EntityFrameworkCore;
using PeakeyeBackendCase.Models;

namespace PeakeyeBackendCase.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Vulnerability> Vulnerabilities { get; set; }
}