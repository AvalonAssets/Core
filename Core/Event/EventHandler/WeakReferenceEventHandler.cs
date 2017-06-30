using System;
using System.Collections.Generic;

namespace AvalonAssets.Core.Event.EventHandler
{
    /// <summary>
    ///     Implementation of <see cref="IEventHandler" />.
    ///     Uses weak reference to hold the reference to subscriber.
    /// </summary>
    internal abstract class WeakReferenceEventHandler : IEventHandler
    {
        private readonly WeakReference _weakReference;

        protected WeakReferenceEventHandler(ISubscriber subscriber)
        {
            _weakReference = new WeakReference(subscriber);
        }

        /// <inheritdoc />
        public bool Alive => _weakReference.Target != null;

        /// <inheritdoc />
        public abstract IEnumerable<Type> Types { get; }

        /// <inheritdoc />
        public bool Matches(object instance)
        {
            return _weakReference.Target == instance;
        }

        /// <inheritdoc />
        public bool Handle(Type messageType, object message)
        {
            if (!Alive)
                return false;
            HandleMessage(messageType, _weakReference.Target, message);
            return true;
        }

        /// <summary>
        ///     Notify message to subscriber.
        /// </summary>
        /// <param name="handlerType">Message type.</param>
        /// <param name="target">Subscriber</param>
        /// <param name="message">Message to be handle.</param>
        protected abstract void HandleMessage(Type handlerType, object target, object message);
    }
}