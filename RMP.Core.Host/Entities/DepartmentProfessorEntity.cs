namespace RMP.Core.Host.Entities;

public class DepartmentProfessorEntity
{
   
    public Guid DepartmentId { get; set; }
    public Guid ProfessorId { get; set; }
    public virtual ProfessorEntity Professor { get; set; }
    public DepartmentEntity Department { get; set; }
   
    
}