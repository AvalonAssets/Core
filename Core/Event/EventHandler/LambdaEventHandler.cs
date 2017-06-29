using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AvalonAssets.Core.Event.EventHandler
{
    /// <summary>
    ///     Implementation of <see cref="WeakReferenceEventHandler" />.
    ///     Uses lambda to find avaiable type.
    /// </summary>
    internal class LambdaEventHandler : WeakReferenceEventHandler
    {
        private readonly Dictionary<Type, Action<object, object>> _supportedHandlers;

        public LambdaEventHandler(ISubscriber subscriber) : base(subscriber)
        {
            _supportedHandlers = new Dictionary<Type, Action<object, object>>();
            // Gets all the ISubscriber<T> interface
            var interfaces = subscriber.GetType().GetInterfaces()
                .Where(x => typeof(ISubscriber).IsAssignableFrom(x) && x.IsGenericType);
            foreach (var @interface in interfaces)
            {
                var type = @interface.GetGenericArguments()[0];
                _supportedHandlers[type] = CreateLambda(type);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Type> Types => _supportedHandlers.Keys;

        /// <inheritdoc />
        protected override void HandleMessage(Type handlerType, object target, object message)
        {
            _supportedHandlers[handlerType](target, message);
        }

        private static Action<object, object> CreateLambda(Type type)
        {
            var genericType = typeof(ISubscriber<>).MakeGenericType(type);
            var method = genericType.GetMethod("Receive", BindingFlags.Instance | BindingFlags.Public);
            var instanceExpression = Expression.Parameter(typeof(object), "instance");
            var messageExpression = Expression.Parameter(typeof(object), "message");
            var instanceCastingExpression = Expression.Convert(instanceExpression, genericType);
            var messageCastingExpression = Expression.Convert(messageExpression, type);
            var invokeExpression = Expression.Call(instanceCastingExpression, method, messageCastingExpression);
            var lambda =
                Expression.Lambda<Action<object, object>>(invokeExpression, instanceExpression, messageExpression)
                    .Compile();
            return lambda;
        }
    }
}