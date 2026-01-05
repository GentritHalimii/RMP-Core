using Riok.Mapperly.Abstractions;
using RMP.Core.Host.Entities;
using RMP.Core.Host.Features.AdminDashboard.GetHighestRatedProfessor;
using RMP.Core.Host.Features.AdminDashboard.GetOldestUniversity;
using RMP.Core.Host.Features.AdminDashboard.GetHighestRatedUniversity;


namespace RMP.Core.Host.Mapper;

[Mapper]
public static partial class AdminDashboardMapper
{
  public static partial GetOldestUniversityResult ToGetOldestUniversityResult(this UniversityEntity university);
  public static partial GetOldestUniversityResponse ToGetOldestUniversityResponse(this GetOldestUniversityResult result);
  public static partial GetHighestRatedUniversityResult ToGetHighestRatedUniversityResult(this RateUniversityEntity rateUniversity);
  public static partial GetHighestRatedUniversityResponse ToGetHighestRatedUniversityResponse(this GetHighestRatedUniversityResult result);
  public static partial GetHighestRatedProfessorResult ToGetHighestRatedProfessorResult(this RateProfessorEntity rateProfessor);
  public static partial GetHighestRatedProfessorResponse ToGetHighestRatedProfessorResponse(this GetHighestRatedProfessorResult result);

}