using RMP.Core.Host.Abstractions.Errors;

namespace RMP.Core.Host.Features.Course;

public static class CourseErrors
{
    public static Error NotFound(Guid id) =>
        new("Courses.NotFound", $"The course with Id '{id}' was not found");
}