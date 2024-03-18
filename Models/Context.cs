#pragma warning disable CS8618

using Microsoft.EntityFrameworkCore;
namespace chore_tracker.Models;

public class Context: DbContext 
{ 
    public Context(DbContextOptions options) : base(options) { }
    
    public DbSet<User> Users { get; set; } 
    public DbSet<Job> Jobs { get; set; } 
    public DbSet<UserJob> UserJobs { get; set; } 

}