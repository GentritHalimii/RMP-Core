using RMP.Host.Abstarctions.Errors;

namespace RMP.Host.Features.News;

public static class NewsErrors
{
    public static Error NotFound(Guid id) =>
        new("News.NotFound", $"The news with Id '{id}' was not found");
    
    public static Error NotFoundNews() =>
        new("News.NotFoundNews", $"The news  wasn't not found");

    
}