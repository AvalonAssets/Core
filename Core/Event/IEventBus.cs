using System;

namespace AvalonAssets.Core.Event
{
    /// <summary>
    ///     <see cref="IEventBus" /> allows object implements <see cref="ISubscriber{T}" /> to
    ///     subscribe and receive corresponding messages published through this aggregator.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        ///     <paramref name="subscriber" /> subscribes to <see cref="ISubscriber{T}" /> that it implemented.
        ///     For example, if it implemented <see cref="ISubscriber{T}" /> of <see cref="string" />.
        ///     It will receives any published messages that is <see cref="string" /> or its subclass.
        /// </summary>
        /// <param name="subscriber">Object that implements <see cref="ISubscriber{T}" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscriber" /> is null.</exception>
        void Subscribe(ISubscriber subscriber);

        /// <summary>
        ///     <paramref name="subscriber" /> unsubscribes from all <see cref="ISubscriber{T}" />.
        ///     If <paramref name="subscriber" /> does not subscribe, it will be ignored.
        /// </summary>
        /// <param name="subscriber">Object that already subscribes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscriber" /> is null.</exception>
        void Unsubscribe(ISubscriber subscriber);

        /// <summary>
        ///     Publishs a <paramref name="message" /> to all the registered <see cref="ISubscriber{T}" /> of
        ///     <typeparamref name="T" /> or its super class.
        /// </summary>
        /// <param name="message">Message to be published.</param>
        /// <typeparam name="T">Event type.</typeparam>
        void Publish<T>(T message) where T : IEvent;
    }
}