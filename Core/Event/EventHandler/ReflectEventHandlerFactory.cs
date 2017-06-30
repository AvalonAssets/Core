namespace AvalonAssets.Core.Event.EventHandler
{
    /// <summary>
    ///     <see cref="ReflectEventHandlerFactory" /> wraps <see cref="ISubscriber{T}" /> to <see cref="IEventHandler" />.
    ///     Uses reflection to find avaiable type and weak reference to handle subscriber.
    /// </summary>
    public class ReflectEventHandlerFactory : IEventHandlerFactory
    {
        /// <inheritdoc />
        public IEventHandler Create(ISubscriber subscriber)
        {
            return new ReflectEventHandler(subscriber);
        }
    }
}