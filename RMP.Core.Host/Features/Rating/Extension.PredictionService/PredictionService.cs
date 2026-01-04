using RMP.Core.Host.Features.Rating.Extension.PredictionService.Interface;

namespace RMP.Core.Host.Features.Rating.Extension.PredictionService;

public class PredictionService(RMP.Core.Host.PredictionService.PredictionServiceClient grpcClient)
    : IPredictionService
{
    public async Task<PredictionResult> PredictToxicityAsync(string feedback)
    {
        var request = new PredictionRequest { SentimentText = feedback };
        var response = grpcClient.PredictToxicity(request);

        return new PredictionResult
        {
            IsToxic = response.IsToxic,
            Message = response.Message
        };
    }
}