using RMP.Host.Entities.Identity;

namespace RMP.Host.Entities;

public abstract class BaseRateEntity : BaseEntity
{
    public int UserId { get; set; }
    
    public int Overall { get; set; }
    
    public required string Feedback { get; set; }
    
    public virtual UserEntity User { get; set; }
}