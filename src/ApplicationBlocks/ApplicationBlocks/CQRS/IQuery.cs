using MediatR;

namespace ApplicationBlocks.CQRS;

// design to return a result operation for the read result
public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull
{
}