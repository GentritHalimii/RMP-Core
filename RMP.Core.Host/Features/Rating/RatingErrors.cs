using RMP.Core.Host.Abstractions.Errors;

namespace RMP.Core.Host.Features.Rating;

public static class RatingErrors
{
    public static Error IsToxic(string message) =>
        new("Toxic", message);
}