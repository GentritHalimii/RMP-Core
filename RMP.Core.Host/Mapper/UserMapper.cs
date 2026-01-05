using Riok.Mapperly.Abstractions;
using RMP.Host.Entities.Identity;
using RMP.Host.Features.User.GetAdmins;
using RMP.Host.Features.User.GetStudents;
using RMP.Host.Features.User.GetUserById;

namespace RMP.Host.Mapper;

[Mapper]
public static partial class UserMapper
{
    public static partial GetUserByIdResult ToGetUserByIdResult(this UserEntity user);
    public static partial GetUserByIdResponse ToGetUserByIdResponse(this GetUserByIdResult result);
    public static partial GetAdminsResult ToGetAdminsResult(this UserEntity user);
    public static partial GetAdminsResponse ToGetAdminsResponse(this GetAdminsResult result);
    
    public static partial GetStudentsResult ToGetStudentsResult(this UserEntity user);
}