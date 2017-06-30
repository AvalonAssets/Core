using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Core.Event.EventHandler;

namespace AvalonAssets.Core.Event
{
    /// <summary>
    ///     <see cref="BasicEventBus" /> is basic implementation of <see cref="IEventBus" />.
    ///     It uses <see cref="IEventHandlerFactory" /> to create <see cref="IEventHandler" /> to wrap subscriber.
    ///     <see cref="Builder" /> is needed for creating <see cref="BasicEventBus" />.
    /// </summary>
    /// <see cref="Builder" />
    public class BasicEventBus : IEventBus
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;

        // Registered event handlers
        private readonly ConcurrentDictionary<Type, HashSet<IEventHandler>> _eventHandlers;

        private BasicEventBus(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _eventHandlers = new ConcurrentDictionary<Type, HashSet<IEventHandler>>();
        }

        /// <inheritdoc />
        public void Subscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            var handler = _eventHandlerFactory.Create(subscriber);
            // Registers if there is atleast one type
            if (!handler.Types.Any())
                return;
            foreach (var type in handler.Types)
            {
                var typeSet = _eventHandlers.GetOrAdd(type, k => new HashSet<IEventHandler>());
                typeSet.Add(handler);
            }
        }

        /// <inheritdoc />
        public void Unsubscribe(ISubscriber subscriber)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            var handler = _eventHandlerFactory.Create(subscriber);
            if (!handler.Types.Any())
                return;
            foreach (var type in handler.Types)
            {
                if (!_eventHandlers.ContainsKey(type))
                    continue;
                var typeSet = _eventHandlers.GetOrAdd(type, k => new HashSet<IEventHandler>());
                lock (typeSet)
                {
                    typeSet.RemoveWhere(h => h.Matches(subscriber));
                }
            }
        }

        /// <inheritdoc />
        public void Publish<T>(T message) where T : IEvent
        {
            var messageType = typeof(T);
            if (!_eventHandlers.ContainsKey(messageType))
                return;
            var toNotify = _eventHandlers[messageType].Where(h => h.CanHandle(messageType)).ToArray();
            // Publishes message
            var dead = toNotify.Where(h => !h.Handle(messageType, message)).ToList();
            if (!dead.Any()) return;
            // Clean up
            foreach (var handler in dead)
            foreach (var type in handler.Types)
            {
                var typeSet = _eventHandlers.GetOrAdd(type, k => new HashSet<IEventHandler>());
                lock (typeSet)
                {
                    typeSet.Remove(handler);
                }
            }
        }

        /// <summary>
        ///     <see cref="Builder" /> is a builder for <see cref="BasicEventBus" />.
        /// </summary>
        public class Builder
        {
            /// <summary>
            ///     <see cref="IEventHandlerFactory" /> that will be used by <see cref="BasicEventBus" />.
            /// </summary>
            public IEventHandlerFactory EventHandlerFactory { private get; set; }

            /// <summary>
            ///     Create a new instance of <see cref="BasicEventBus" />.
            /// </summary>
            /// <returns>A new instance of <see cref="BasicEventBus" />.</returns>
            public BasicEventBus Build()
            {
                return new BasicEventBus(EventHandlerFactory ?? new ReflectEventHandlerFactory());
            }
        }
    }
}