using MediatR;
namespace ApplicationBlocks.CQRS;

// does not produce/return response
public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
{
}

// does produce/return response and it cannot be null
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}