using System.Reflection;
using Microsoft.EntityFrameworkCore;
using GoodMarket.Identity.Api.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GoodMarket.Identity.Api.Data;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}