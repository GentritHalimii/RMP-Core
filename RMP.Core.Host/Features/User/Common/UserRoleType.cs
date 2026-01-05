using System.ComponentModel.DataAnnotations;

namespace RMP.Host.Features.User.Common;

public enum UserRoleType : short
{
    [Display(Name = "Admin")]
    Admin = 1,

    [Display(Name = "Student")]
    Student = 2,
}