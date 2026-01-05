namespace RMP.Core.Host.Features.Rating.Extension.PredictionService.Interface;

public interface IPredictionService
{
    Task<PredictionResult> PredictToxicityAsync(string feedback);
}