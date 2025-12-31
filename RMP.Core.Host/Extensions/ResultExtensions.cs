using RMP.Core.Host.Abstractions.Errors;
using RMP.Core.Host.Abstractions.ResultResponse;

namespace RMP.Core.Host.Extensions;

public static class ResultExtensions
{
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure) =>
        result.IsSuccess ? onSuccess() : onFailure(result.Error);
}