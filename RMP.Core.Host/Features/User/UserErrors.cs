using RMP.Core.Host.Abstractions.Errors;

namespace RMP.Core.Host.Features.User;

public static class UserErrors
{
    public static Error UserNotFound(int id) =>
        new("Users.NotFound", $"The user with Id '{id}' was not found");
    
    public static Error AlreadyExist(string email) =>
        new("Users.AlreadyExist", $"The user with email '{email}' already exist");
    
    public static Error RegistrationFailed(string code, string description) =>
        new(code, description);
    
    public static Error LoginFailed() =>
        new("LoginFailed", $"Invalid login attempt.");
    
    public static Error AuthenticationFailed() =>
        new("AuthenticationFailed", $"Authentication failed.");
    
    
}