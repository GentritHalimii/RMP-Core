using RMP.Host.Abstarctions.Errors;

namespace RMP.Host.Features.University;

public static class UniversityErrors
{
    public static Error NotFound(Guid id) =>
        new("Universities.NotFound", $"The university with Id '{id}' was not found");
}