using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RMP.Core.Host.Abstractions.CQRS;
using RMP.Core.Host.Abstractions.ResultResponse;
using RMP.Core.Host.Database;
using RMP.Host.Abstarctions.CQRS;
using RMP.Host.Abstarctions.ResultResponse;
using RMP.Host.Database;
using RMP.Host.Features.News.DeleteNews;
using RMP.Host.Mapper;

namespace RMP.Host.Features.News.UpdateNews;

public sealed record UpdateNewsCommand(
    Guid Id,
    string Title,
    string Content,
    DateTime PublicationDate,
    string Category,
    string ProfilePhotoPath) : ICommand<Result<UpdateNewsResult>>;

public sealed record UpdateNewsResult(bool IsSuccess);

public sealed class UpdateNewsCommandValidator : AbstractValidator<UpdateNewsCommand>
{
    public UpdateNewsCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required!");
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required!");
        RuleFor(x => x.PublicationDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Publication date must be in the past or present.");
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required!");
        RuleFor(x => x.ProfilePhotoPath)
            .NotEmpty()
            .WithMessage("Profile photo path is required!");
    }
}

internal sealed class UpdateNewsCommandHandler(ApplicationDbContext dbContext) : ICommandHandler<UpdateNewsCommand, Result<UpdateNewsResult>>
{
    public async Task<Result<UpdateNewsResult>> Handle(UpdateNewsCommand command, CancellationToken cancellationToken)
    {
        var news = await dbContext
            .News
            .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

        if (news is null)
            return Result.Failure<UpdateNewsResult>(NewsErrors.NotFound(command.Id));
        
        command.ToNewsEntity(news);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateNewsResult(true);
    }
}