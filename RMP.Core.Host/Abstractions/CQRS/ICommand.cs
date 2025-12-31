using MediatR;

namespace RMP.Core.Host.Abstractions.CQRS;

public interface ICommand : ICommand<Unit>;

public interface ICommand<out TResponse> : IRequest<TResponse>;