using GoodMarket.Shared.Mediator.Command;
using GoodMarket.Shared.Mediator.Query;

namespace GoodMarket.Shared.Mediator;

public interface IMediator
{
    Task<TResult> SendCommandAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>;

    Task<TResult> SendQueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>;
}