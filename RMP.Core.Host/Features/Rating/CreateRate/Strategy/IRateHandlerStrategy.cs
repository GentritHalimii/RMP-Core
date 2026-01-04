using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;

namespace RMP.Core.Host.Features.Rating.CreateRate.Strategy;

public interface IRateHandlerStrategy
{
    Task<Result<RateResult>> HandleAsync(
        CreateRateRequest request, 
        ApplicationDbContext dbContext, 
        CancellationToken cancellationToken);
}