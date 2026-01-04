using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Entities;
using RMP.Core.Host.Entities.Identity;

namespace RMP.Core.Host.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
	: IdentityDbContext<UserEntity, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
	public DbSet<UniversityEntity> Universities { get; set; }
	public DbSet<DepartmentEntity> Departments { get; set; }
	public DbSet<ProfessorEntity> Professors { get; set; }
	public DbSet<RateUniversityEntity> RateUniversities { get; set; }
	public DbSet<RateProfessorEntity> RateProfessors { get; set; }
	public DbSet<NewsEntity> News { get; set; }
	public DbSet<CourseEntity> Courses { get; set; }
	public DbSet<ProfessorCourseEntity> ProfessorCourses { get; set; }
	public DbSet<DepartmentProfessorEntity> DepartmentProfessors { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new RoleConfiguration());
		modelBuilder.ApplyConfiguration(new RoleConfiguration.RoleSeedData());
		modelBuilder.Entity<UserRole>().ToTable("UserRoles");
		modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims");
		modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
		modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
		modelBuilder.Entity<UserToken>().ToTable("UserTokens");
		modelBuilder.Entity<UniversityEntity>().ToTable("Universities");
		modelBuilder.Entity<DepartmentEntity>().ToTable("Departments");
		modelBuilder.Entity<ProfessorEntity>().ToTable("Professors");
		modelBuilder.Entity<RateUniversityEntity>().ToTable("RateUniversities");
		modelBuilder.Entity<RateProfessorEntity>().ToTable("RateProfessors");
		modelBuilder.Entity<NewsEntity>().ToTable("News");
		modelBuilder.Entity<CourseEntity>().ToTable("Courses");
		modelBuilder.Entity<ProfessorCourseEntity>().ToTable("ProfessorCourses");
		modelBuilder.Entity<DepartmentProfessorEntity>().ToTable("DepartmentProfessors");

		modelBuilder.Entity<UniversityEntity>()
			.HasKey(pk => new { pk.Id });
		modelBuilder.Entity<DepartmentEntity>()
			.HasKey(pk => new { pk.Id });
		modelBuilder.Entity<ProfessorEntity>()
			.HasKey(pk => new { pk.Id });

		/// === UniversityEntity === ///
		modelBuilder.Entity<UniversityEntity>()
			.HasMany(u => u.Departments)
			.WithOne(d => d.University)
			.HasForeignKey(d => d.UniversityId)
			.OnDelete(DeleteBehavior.Cascade);

		// === UserEntity Relationships === //
		modelBuilder.Entity<UserEntity>()
			.HasOne(u => u.University)
			.WithMany()
			.HasForeignKey(u => u.UniversityId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<UserEntity>()
			.HasOne(u => u.Department)
			.WithMany(d => d.Users)
			.HasForeignKey(u => u.DepartmentId)
			.OnDelete(DeleteBehavior.Restrict);

		// === DepartmentEntity === //
		modelBuilder.Entity<DepartmentEntity>()
			.HasOne(d => d.University)
			.WithMany(u => u.Departments)
			.HasForeignKey(d => d.UniversityId)
			.OnDelete(DeleteBehavior.Cascade);

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

		// === DepartmentProfessorEntity Relationships === //
		modelBuilder.Entity<DepartmentProfessorEntity>()
			.HasKey(dp => new { dp.DepartmentId, dp.ProfessorId });

		modelBuilder.Entity<DepartmentProfessorEntity>()
			.HasOne(dp => dp.Department)
			.WithMany(d => d.DepartmentProfessors)
			.HasForeignKey(dp => dp.DepartmentId);

		modelBuilder.Entity<DepartmentProfessorEntity>()
			.HasOne(dp => dp.Professor)
			.WithMany(p => p.DepartmentProfessors)
			.HasForeignKey(dp => dp.ProfessorId);

		modelBuilder.Entity<CourseEntity>()
		   .HasOne(r => r.Department)
		   .WithMany()
		   .HasForeignKey(r => r.DepartmentID)
		   .OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<ProfessorCourseEntity>()
			.HasKey(dp => new { dp.CourseId, dp.ProfessorId });

		modelBuilder.Entity<ProfessorCourseEntity>()
			.HasOne(r => r.Course)
			.WithMany()
			.HasForeignKey(r => r.CourseId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<ProfessorCourseEntity>()
			.HasOne(r => r.Professor)
			.WithMany()
			.HasForeignKey(r => r.ProfessorId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
