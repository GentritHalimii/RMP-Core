using FluentValidation;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Mapper;

namespace RMP.Host.Features.News.CreateNews;

public sealed record CreateNewsCommand(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath) : ICommand<Result<CreateNewsResult>>;

public sealed record CreateNewsResult(Guid Id);

public sealed class CreateNewsCommandValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required!");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required!");

        RuleFor(x => x.PublicationDate)
            .NotEmpty()
            .WithMessage("Publication date is required!")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Publication date cannot be in the future.");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required!");

        RuleFor(x => x.ProfilePhotoPath)
            .NotEmpty()
            .WithMessage("Profile photo path is required!");
    }
}

internal sealed class CreateNewsCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<CreateNewsCommand, Result<CreateNewsResult>>
{
    public async Task<Result<CreateNewsResult>> Handle(CreateNewsCommand command, CancellationToken cancellationToken)
    {
        var news = command.ToNewsEntity();
        
        dbContext.News.Add(news);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new CreateNewsResult(news.Id);
    }
}