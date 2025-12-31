using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Entities;

namespace RMP.Core.Host.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
	public DbSet<ProfessorEntity> Professors { get; set; }
}