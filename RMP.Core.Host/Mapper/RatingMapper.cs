using Riok.Mapperly.Abstractions;
using RMP.Core.Host.Entities;
using RMP.Core.Host.Features.Rating.GetRateProfessors;
using RMP.Core.Host.Features.Rating.GetRateProfessorsById;
using RMP.Core.Host.Features.Rating.GetRateProfessorsByProfessorId;
using RMP.Core.Host.Features.Rating.GetRateProfessorsByStudentId;
using RMP.Core.Host.Features.Rating.GetRateUniversities;

namespace RMP.Core.Host.Mapper;

[Mapper]
public static partial class RatingMapper
{
    public static partial GetRateProfessorsResult ToGetRateProfessorsResult(this RateProfessorEntity rateProfessor);
    public static partial GetRateProfessorsResponse ToGetRateProfessorsResponse(this GetRateProfessorsResult result);
    public static partial GetRateUniversitiesResult ToGetRateUniversitiesResult(this RateUniversityEntity rateUniversity);
    public static partial GetRateUniversitiesResponse ToGetRateUniversitiesResponse(this GetRateUniversitiesResult result);
    public static partial GetRateProfessorsByIdResult ToGetRateProfessorsByIdResult(this RateProfessorEntity rateProfessor);
    public static partial GetRateProfessorsByIdResponse ToGetRateProfessorsByIdResponse(this GetRateProfessorsByIdResult result);
    public static partial GetRateProfessorsByProfessorIdResult ToGetRateProfessorsByProfessorIdResult(this RateProfessorEntity rateProfessor);
    public static partial GetRateProfessorsByProfessorIdResponse ToGetRateProfessorsByProfessorIdResponse(this GetRateProfessorsByProfessorIdResult result);
    public static partial GetRateProfessorsByStudentIdResult ToGetRateProfessorsByStudentIdResult(this RateProfessorEntity rateProfessor);
    public static partial GetRateProfessorsByStudentIdResponse ToGetRateProfessorsByStudentIdResponse(this GetRateProfessorsByStudentIdResult result);
}
