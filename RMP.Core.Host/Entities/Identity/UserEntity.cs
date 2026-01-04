using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RMP.Core.Host.Entities.Identity;

public class UserEntity : IdentityUser<int>
{
    /// <summary>
    /// The first name of the user.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The last name or surname of the user.
    /// </summary>
    public required string Surname { get; set; }
    
    /// <summary>
    /// The identifier of the associated university.
    /// </summary>
    public required Guid UniversityId { get; set; }
    
    /// <summary>
    /// The identifier of the department the user belongs to.
    /// </summary>
    public required Guid DepartmentId { get; set; }

    /// <summary>
    /// The grade or level of the user within their academic program.
    /// </summary>
    public int? Grade { get; set; }

    /// <summary>
    /// Path to the profile photo of the user, if available.
    /// </summary>
    public string? ProfilePhotoPath { get; set; }

    /// <summary>
    /// The university associated with the user.
    /// </summary>
    public virtual UniversityEntity? University { get; set; }
    
    /// <summary>
    /// The department associated with the user.
    /// </summary>
    public DepartmentEntity? Department { get; set; }
    
    public RateUniversityEntity? RateUniversity { get; set; }

    public ICollection<RateProfessorEntity>? RateProfessors { get; set; }
}

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasOne(x => x.University)
            .WithMany()
            .HasForeignKey(x => x.UniversityId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}