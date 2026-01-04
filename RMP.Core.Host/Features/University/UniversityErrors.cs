using RMP.Core.Host.Abstractions.Errors;

namespace RMP.Core.Host.Features.University;

public static class UniversityErrors
{
    public static Error NotFound(Guid id) =>
        new("Universities.NotFound", $"The university with Id '{id}' was not found");
}