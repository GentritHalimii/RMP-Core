using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Entities;

namespace RMP.Core.Host.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
	public DbSet<UniversityEntity> Universities { get; set; }
	public DbSet<ProfessorEntity> Professors { get; set; }
	public DbSet<CourseEntity> Courses { get; set; }
	public DbSet<ProfessorCourseEntity> ProfessorCourses { get; set; }
	public DbSet<RateUniversityEntity> RateUniversities { get; set; }
	public DbSet<RateProfessorEntity> RateProfessors { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		
		modelBuilder.Entity<RateUniversityEntity>().ToTable("RateUniversities");
		modelBuilder.Entity<RateProfessorEntity>().ToTable("RateProfessors");
		
		// === RateUniversityEntity Relationships === //
		modelBuilder.Entity<RateUniversityEntity>()
			.HasOne(r => r.User)
			.WithMany()
			.HasForeignKey(r => r.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<RateUniversityEntity>()
			.HasOne(r => r.University)
			.WithMany()
			.HasForeignKey(r => r.UniversityId)
			.OnDelete(DeleteBehavior.Cascade);

		// === RateProfessorEntity Relationships === //
		modelBuilder.Entity<RateProfessorEntity>()
			.HasOne(r => r.User)
			.WithMany()
			.HasForeignKey(r => r.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<RateProfessorEntity>()
			.HasOne(r => r.Professor)
			.WithMany()
			.HasForeignKey(r => r.ProfessorId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}