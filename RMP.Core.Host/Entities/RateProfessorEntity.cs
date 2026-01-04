namespace RMP.Core.Host.Entities;

public class RateProfessorEntity : BaseRateEntity
{
    public Guid ProfessorId { get; set; }
    
    public int? CommunicationSkills { get; set; }
    
    public int? Responsiveness { get; set; }
    
    public int? GradingFairness { get; set; }
    
    public virtual ProfessorEntity Professor { get; set; }
}