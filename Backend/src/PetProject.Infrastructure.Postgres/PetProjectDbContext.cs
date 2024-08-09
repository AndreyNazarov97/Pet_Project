using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.Domain.Entities;

namespace PetProject.Infrastructure.Postgres;

public class PetProjectDbContext : DbContext
{
    public PetProjectDbContext(DbContextOptions<PetProjectDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Volunteer> Volunteers { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
    
}