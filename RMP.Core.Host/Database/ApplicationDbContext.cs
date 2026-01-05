using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Entities;
using RMP.Host.Entities;

namespace RMP.Core.Host.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
	public DbSet<UniversityEntity> Universities { get; set; }
	public DbSet<ProfessorEntity> Professors { get; set; }
	public DbSet<CourseEntity> Courses { get; set; }
	public DbSet<ProfessorCourseEntity> ProfessorCourses { get; set; }
}