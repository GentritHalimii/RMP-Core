using RMP.Core.Host.Abstractions.Errors;

namespace RMP.Core.Host.Features.Proffesor;

public static class ProfessorErrors
{
    public static Error NotFound(Guid id) =>
        new("Proffesors.NotFound", $"The proffesor with Id '{id}' was not found");
}