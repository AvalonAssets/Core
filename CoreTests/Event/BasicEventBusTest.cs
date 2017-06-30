using System;
using System.Collections.Generic;
using AvalonAssets.Core.Event;
using AvalonAssets.Core.Event.EventHandler;
using NSubstitute;
using NUnit.Framework;

namespace AvalonAssets.CoreTests.Event
{
    [TestFixture]
    public class BasicEventBusTest
    {
        [SetUp]
        public void Setup()
        {
            _intIListSubscriber = Substitute.For<ISubscriber<IEvent<IList<int>>>>();
            _intIListHandler = Substitute.For<IEventHandler>();
            _intIListHandler.Alive.Returns(true);
            _intIListHandler.Types.Returns(new List<Type> {typeof(IEvent<IList<int>>)});
            _intIListHandler.Matches(Arg.Any<ISubscriber>()).Returns(false);
            _intIListHandler.Matches(Arg.Is<ISubscriber>(_intIListSubscriber)).Returns(true);
            _intIListHandler.Handle(Arg.Any<Type>(), Arg.Any<object>()).Returns(callinfo =>
            {
                var message = callinfo.ArgAt<IEvent<IList<int>>>(1);
                _intIListSubscriber.Receive(message);
                return true;
            });
            _intListSubscriber = Substitute.For<ISubscriber<IEvent<List<int>>>>();
            _intListHandler = Substitute.For<IEventHandler>();
            _intListHandler.Alive.Returns(true);
            _intListHandler.Types.Returns(new List<Type> {typeof(IEvent<List<int>>)});
            _intListHandler.Matches(Arg.Any<ISubscriber>()).Returns(false);
            _intListHandler.Matches(Arg.Is<ISubscriber>(_intListSubscriber)).Returns(true);
            _intListHandler.Handle(Arg.Any<Type>(), Arg.Any<object>()).Returns(callinfo =>
            {
                var message = callinfo.ArgAt<IEvent<List<int>>>(1);
                _intListSubscriber.Receive(message);
                return true;
            });
            var eventHandlerFactory = Substitute.For<IEventHandlerFactory>();
            eventHandlerFactory.Create(Arg.Is<ISubscriber>(_intIListSubscriber)).Returns(_intIListHandler);
            eventHandlerFactory.Create(Arg.Is<ISubscriber>(_intListSubscriber)).Returns(_intListHandler);
            var builder = new BasicEventBus.Builder {EventHandlerFactory = eventHandlerFactory};
            _eventBus = builder.Build();
        }

        private BasicEventBus _eventBus;
        private ISubscriber<IEvent<IList<int>>> _intIListSubscriber;
        private ISubscriber<IEvent<List<int>>> _intListSubscriber;
        private IEventHandler _intIListHandler;
        private IEventHandler _intListHandler;

        [Test]
        public void SubscribePublishTest()
        {
            _eventBus.Subscribe(_intIListSubscriber);
            _eventBus.Subscribe(_intListSubscriber);
            var listEventStub = Substitute.For<IEvent<List<int>>>();
            _eventBus.Publish(listEventStub);
            _intIListSubscriber.DidNotReceive().Receive(listEventStub);
            _intListSubscriber.Received().Receive(listEventStub);
            var iListEventStub = Substitute.For<IEvent<IList<int>>>();
            _eventBus.Publish(iListEventStub);
            _intIListSubscriber.Received().Receive(iListEventStub);
        }

        [Test]
        public void SubscribeUnScbscribeTest()
        {
            _eventBus.Subscribe(_intIListSubscriber);
            var iListEventStub = Substitute.For<IEvent<IList<int>>>();
            _eventBus.Publish(iListEventStub);
            _intIListSubscriber.Received().Receive(iListEventStub);
            _eventBus.Unsubscribe(_intIListSubscriber);
            var iListEventStub2 = Substitute.For<IEvent<IList<int>>>();
            _eventBus.Publish(iListEventStub2);
            _intIListSubscriber.DidNotReceive().Receive(iListEventStub2);
        }
    }
}