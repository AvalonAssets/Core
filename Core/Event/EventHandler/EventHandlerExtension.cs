using System;
using System.Linq;

namespace AvalonAssets.Core.Event.EventHandler
{
    public static class EventHandlerExtension
    {
        /// <summary>
        ///     Handles <paramref name="message" /> of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Message type.</typeparam>
        /// <param name="handler">Handler.</param>
        /// <param name="message">Message to be handle.</param>
        /// <returns>True if the object is alive.</returns>
        /// <seealso cref="IEventHandler.Handle" />
        public static bool Handle<T>(this IEventHandler handler, T message)
        {
            return handler.Handle(typeof(T), message);
        }

        /// <summary>
        ///     Checks if <paramref name="handler" /> can handle message of <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Message type.</typeparam>
        /// <param name="handler">Handler.</param>
        /// <returns>True if it can handle <typeparamref name="T" />.</returns>
        /// <seealso cref="CanHandle" />
        public static bool CanHandle<T>(this IEventHandler handler)
        {
            return handler.CanHandle(typeof(T));
        }

        /// <summary>
        ///     Checks if <paramref name="handler" /> can handle message of <paramref name="messageType" />.
        /// </summary>
        /// <param name="handler">Handler.</param>
        /// <param name="messageType">Message type.</param>
        /// <returns>True if it can handle <paramref name="messageType" />.</returns>
        /// <seealso cref="CanHandle{T}" />
        public static bool CanHandle(this IEventHandler handler, Type messageType)
        {
            return handler.Types.Contains(messageType);
        }
    }
}