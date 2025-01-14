using MediatR;

namespace ApplicationBlocks.CQRS;

// does not produce/return a reponse
public interface ICommand : ICommand<Unit> // Unit = void type
{
}

// does produce/return a response
public interface ICommand<out TResponse> : IRequest<TResponse> // interface to define a contract for all command types.
{
}