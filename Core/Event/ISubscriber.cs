namespace AvalonAssets.Core.Event
{
    /// <summary>
    ///     Marker interface.
    /// </summary>
    /// <remarks>
    ///     Do not use this. Use <see cref="ISubscriber{T}" /> instead.
    /// </remarks>
    public interface ISubscriber
    {
    }

    /// <summary>
    ///     Subscriber need to implement <see cref="ISubscriber{T}" /> to subscribe to message type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <seealso cref="IEventBus" />
    public interface ISubscriber<in T> : ISubscriber where T : IEvent
    {
        /// <summary>
        ///     Receives publish messages from <see cref="IEventBus" />.
        /// </summary>
        /// <param name="message">Message received.</param>
        void Receive(T message);
    }
}