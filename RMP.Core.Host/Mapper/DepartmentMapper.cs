using Riok.Mapperly.Abstractions;
using RMP.Core.Host.Entities;
using RMP.Core.Host.Features.Department.CreateDepartment;
using RMP.Core.Host.Features.Department.UpdateDepartment;
using RMP.Core.Host.Features.Department.GetDepartments;
using RMP.Core.Host.Features.Department.GetDepartmentById;




namespace RMP.Core.Host.Mapper;

[Mapper]
public static partial class DepartmentMapper
{
    // public static partial DepartmentEntity ToDepartmentEntity(this CreateDepartmentCommand command);
    //
    // public static partial GetDepartmentByIdResponse ToGetDepartmentByIdResponse(this GetDepartmentByIdResult result);
    // public static partial GetDepartmentByIdResult ToGetDepartmentByIdResult(this DepartmentEntity department);


        public static partial DepartmentEntity ToDepartmentEntity(this CreateDepartmentCommand command);
        public static partial void ToDepartmentEntity(this UpdateDepartmentCommand command, DepartmentEntity entity);
        public static partial UpdateDepartmentCommand ToUpdateDepartmentCommand(this UpdateDepartmentRequest request);
        public static partial GetDepartmentByIdResult ToGetDepartmentByIdResult(this DepartmentEntity Department);
        public static partial GetDepartmentByIdResponse ToGetDepartmentByIdResponse(this GetDepartmentByIdResult result);
        public static partial GetDepartmentsResult ToGetDepartmentsResult(this DepartmentEntity Department);
        public static partial GetDepartmentsResponse ToGetDepartmentsResponse(this GetDepartmentsResult result);
    }
