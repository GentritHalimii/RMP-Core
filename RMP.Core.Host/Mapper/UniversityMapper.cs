using Riok.Mapperly.Abstractions;
using RMP.Host.Entities;
using RMP.Host.Features.University.CreateUniversity;
using RMP.Host.Features.University.GetUniversities;
using RMP.Host.Features.University.GetUniversityById;
using RMP.Host.Features.University.UpdateUniversity;

namespace RMP.Host.Mapper;

[Mapper]
public static partial class UniversityMapper
{
    public static partial UniversityEntity ToUniversityEntity(this CreateUniversityCommand command);
    public static partial void ToUniversityEntity(this UpdateUniversityCommand command, UniversityEntity entity);
    public static partial UpdateUniversityCommand ToUpdateUniversityCommand(this UpdateUniversityRequest request);
    public static partial GetUniversityByIdResult ToGetUniversityByIdResult(this UniversityEntity university);
    public static partial GetUniversityByIdResponse ToGetUniversityByIdResponse(this GetUniversityByIdResult result);
    public static partial GetUniversitiesResult ToGetUniversitiesResult(this UniversityEntity university);
    public static partial GetUniversitiesResponse ToGetUniversitiesResponse(this GetUniversitiesResult result);
}