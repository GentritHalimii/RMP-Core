using RMP.Host.Abstarctions.Errors;

namespace RMP.Host.Features.Department;

public static class DepartmentErrors
{
    public static Error NotFound(Guid id) =>
        new("Departments.NotFound", $"The department with Id '{id}' was not found");
}