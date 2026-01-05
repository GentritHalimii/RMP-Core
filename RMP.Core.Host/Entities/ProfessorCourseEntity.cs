namespace RMP.Core.Host.Entities;

public class ProfessorCourseEntity
{
    public Guid ProfessorId { get; set; }
    
    public Guid CourseId { get; set; }
    
    public virtual ProfessorEntity Professor { get; set; }
    
    public virtual CourseEntity Course { get; set; }
}