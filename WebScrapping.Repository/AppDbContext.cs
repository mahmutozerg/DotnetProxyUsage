using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebScrapping.Core.Models;

namespace WebScrapping.Repository;

public class AppDbContext:DbContext
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserScrappingRequest> UserScrappingRequests { get; set; }
    public DbSet<ScrappingResults> ScrappingResults { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
}