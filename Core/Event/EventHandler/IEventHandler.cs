using System;
using System.Collections.Generic;

namespace AvalonAssets.Core.Event.EventHandler
{
    /// <summary>
    ///     <see cref="IEventHandler" /> handles the reference of <see cref="ISubscriber" /> and extracts metadata from it.
    /// </summary>
    /// <seealso cref="IEventHandlerFactory" />
    public interface IEventHandler
    {
        /// <summary>
        ///     Checks if the object still available.
        /// </summary>
        /// <returns>True if object is not GC.</returns>
        bool Alive { get; }

        /// <summary>
        ///     Gets All the <see cref="Type" /> that can handle by this <see cref="IEventHandler" />.
        /// </summary>
        /// <returns>All <see cref="Type" /> that this <see cref="IEventHandler" /> that can handle.</returns>
        IEnumerable<Type> Types { get; }

        /// <summary>
        ///     Check if <paramref name="instance" /> equals to its <see cref="ISubscriber" />.
        /// </summary>
        /// <param name="instance">Object to be checked.</param>
        /// <returns>True if this handler is wraping <paramref name="instance" />.</returns>
        bool Matches(object instance);

        /// <summary>
        ///     Handles <paramref name="message" /> of type <paramref name="messageType" />.
        /// </summary>
        /// <param name="messageType">Message type.</param>
        /// <param name="message">Message to be handle.</param>
        /// <returns>True if the object is alive.</returns>
        bool Handle(Type messageType, object message);
    }
}