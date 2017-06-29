using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AvalonAssets.Core.Event.EventHandler;

namespace AvalonAssets.Core.Event
{
    /// <summary>
    ///     Basic implementation of <see cref="IEventBus" />.
    /// </summary>
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

        public class Builder
        {
            public IEventHandlerFactory EventHandlerFactory { private get; set; }

            public BasicEventBus Build()
            {
                return new BasicEventBus(EventHandlerFactory ?? new ReflectEventHandlerFactory());
            }
        }
    }
}