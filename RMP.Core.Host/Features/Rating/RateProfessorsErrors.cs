using RMP.Core.Host.Abstractions.Errors;

namespace RMP.Core.Host.Features.Rating;

public static class RateProfessorsErrors
{
    public static Error NotFound(Guid id) =>
        new("RateProfessors.NotFound", $"The rateProfessors with Id '{id}' was not found");
    
    public static Error NotFound(int id) =>
        new("RateProfessors.NotFound", $"The rateProfessors with Id '{id}' was not found");

    public static Error NotFoundRateProfessors() =>
        new("RateProfessors.NotFoundRateProfessors", $"The rateProfessors  wasn't not found");
}