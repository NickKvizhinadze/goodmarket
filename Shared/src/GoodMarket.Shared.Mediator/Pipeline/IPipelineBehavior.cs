namespace GoodMarket.Shared.Mediator.Pipeline;

public interface IPipelineBehavior<in TInput, TOutput>
{
    Task<TOutput> HandleAsync(TInput input, Func<Task<TOutput>> next, CancellationToken cancellationToken = default);
}