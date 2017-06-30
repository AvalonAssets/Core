namespace AvalonAssets.Core.Event.EventHandler
{
    /// <summary>
    ///     <see cref="LambdaEventHandlerFactory" /> wraps <see cref="ISubscriber{T}" /> to <see cref="IEventHandler" />.
    ///     Uses lambda to find avaiable type and weak reference to handle subscriber.
    /// </summary>
    public class LambdaEventHandlerFactory : IEventHandlerFactory
    {
        /// <inheritdoc />
        public IEventHandler Create(ISubscriber subscriber)
        {
            return new LambdaEventHandler(subscriber);
        }
    }
}