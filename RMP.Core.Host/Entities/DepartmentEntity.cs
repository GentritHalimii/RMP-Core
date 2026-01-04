using RMP.Core.Host.Entities.Identity;

namespace RMP.Core.Host.Entities;

public class DepartmentEntity : BaseEntity
{
    public required string Name { get; set; }
    public Guid UniversityId { get; set; }
    public required int EstablishedYear { get; set; }
    public required string Description { get; set; }
    public int StaffNumber { get; set; }
    public int StudentsNumber { get; set; }
    public int CoursesNumber { get; set; }
    public ICollection<UserEntity> Users { get; set; }
    public UniversityEntity University { get; set; }
    public ICollection<DepartmentProfessorEntity> DepartmentProfessors { get; set; }
    
}