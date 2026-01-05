using Riok.Mapperly.Abstractions;
using RMP.Core.Host.Entities;
using RMP.Core.Host.Features.Course.CreateCourse;
using RMP.Core.Host.Features.Course.UpdateCourse;
using RMP.Core.Host.Features.Course.GetCourses;
using RMP.Core.Host.Features.Course.GetCourseById;

namespace RMP.Core.Host.Mapper;

[Mapper]
public static partial class CourseMapper
{
    public static partial CourseEntity ToCourseEntity(this CreateCourseCommand command);
    public static partial void ToCourseEntity(this UpdateCourseCommand command, CourseEntity entity);
    public static partial UpdateCourseCommand ToUpdateCourseCommand(this UpdateCourseRequest request);
    public static partial GetCourseByIdResult ToGetCourseByIdResult(this CourseEntity Course);
    public static partial GetCourseByIdResponse ToGetCourseByIdResponse(this GetCourseByIdResult result);
    public static partial GetCoursesResult ToGetCoursesResult(this CourseEntity Course);
    public static partial GetCoursesResponse ToGetCoursesResponse(this GetCoursesResult result);
}
