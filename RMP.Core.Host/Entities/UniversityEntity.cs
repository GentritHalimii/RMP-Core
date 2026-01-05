namespace RMP.Core.Host.Entities;

public class UniversityEntity : BaseEntity
{
    public required string Name { get; set; }
    
    public required int EstablishedYear { get; set; }
    
    public required string Description { get; set; }
    
    public int StaffNumber { get; set; }
    
    public int StudentsNumber { get; set; }
    
    public int CoursesNumber { get; set; }
    
    public string? ProfilePhotoPath { get; set; }
    
    public ICollection<DepartmentEntity> Departments { get; set; }
    
    public ICollection<UserEntity> Users { get; set; }
    
    public ICollection<RateUniversityEntity> RateUniversities { get; set; }
}