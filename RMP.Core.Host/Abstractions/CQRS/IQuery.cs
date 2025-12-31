using MediatR;

namespace RMP.Core.Host.Abstractions.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull;