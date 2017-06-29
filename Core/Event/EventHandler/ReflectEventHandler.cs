using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AvalonAssets.Core.Event.EventHandler
{
    /// <summary>
    ///     Implementation of <see cref="WeakReferenceEventHandler" />.
    ///     Uses reflection to find avaiable type.
    /// </summary>
    internal class ReflectEventHandler : WeakReferenceEventHandler
    {
        private readonly Dictionary<Type, MethodInfo> _supportedHandlers;

        public ReflectEventHandler(ISubscriber subscriber) : base(subscriber)
        {
            _supportedHandlers = new Dictionary<Type, MethodInfo>();
            // Gets all the ISubscriber<T> interface
            var interfaces = subscriber.GetType().GetInterfaces()
                .Where(x => typeof(ISubscriber).IsAssignableFrom(x) && x.IsGenericType);
            foreach (var @interface in interfaces)
            {
                var type = @interface.GetGenericArguments()[0];
                var method = @interface.GetMethod("Receive");
                _supportedHandlers[type] = method;
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Type> Types => _supportedHandlers.Keys;

        /// <inheritdoc />
        protected override void HandleMessage(Type handlerType, object target, object message)
        {
            _supportedHandlers[handlerType].Invoke(target, new[] {message});
        }
    }
}