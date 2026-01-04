namespace RMP.Core.Host.Entities;

public class RateUniversityEntity : BaseRateEntity
{
    public Guid UniversityId { get; set; }

    public virtual UniversityEntity University { get; set; }
} 