namespace AvalonAssets.Core.Event.EventHandler
{
    /// <summary>
    ///     <see cref="IEventHandlerFactory" /> wraps <see cref="ISubscriber{T}" /> to <see cref="IEventHandler" />.
    /// </summary>
    /// <seealso cref="IEventHandler" />
    /// <seealso cref="BasicEventBus" />
    public interface IEventHandlerFactory
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="IEventHandler" /> with <paramref name="subscriber" />.
        /// </summary>
        /// <param name="subscriber">Subscriber.</param>
        /// <returns>New instance of <see cref="IEventHandler" />.</returns>
        IEventHandler Create(ISubscriber subscriber);
    }
}