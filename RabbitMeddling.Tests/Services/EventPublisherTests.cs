using AutoFixture;
using Moq;
using Moq.AutoMock;
using Otb.Configuration;
using RabbitMeddling.Services;
using RabbitMeddling.Services.Events;
using Xunit;

namespace RabbitMeddling.Tests.Services
{
    public class EventPublisherTests
    {
        public Fixture AutoFixture { get; set; }
        public AutoMocker Mocker { get; set; }

        public EventPublisherTests()
        {
            AutoFixture = new Fixture();
            Mocker = new AutoMocker();
        }

        public class TestObject
        {
            public int Property;
        }

        [Fact]
        public void WhenPublishGenericEvent()
        {
            //Arrange
            var subject = Mocker.CreateInstance<EventPublisher>();
            var exchangeName = AutoFixture.Create<string>();
            var testObject = AutoFixture.Create<TestObject>();
            var eventName = AutoFixture.Create<string>();
            var eventInstance = new Event<TestObject>(testObject, eventName);

            Mocker.GetMock<IProvideConfiguration>()
                .Setup(getter => getter.GetStringValue("RabbitMq:ExchangeName"))
                .Returns(exchangeName);

            //Act
            subject.Publish(eventInstance);

            //Assert
            Mocker.Verify<IPublishRabbitMqMessage>(publisher =>
                publisher.Publish(
                    exchangeName,
                    $"{eventInstance.Type.GetType().Name.ToLower()}Event",
                    It.Is<Event<TestObject>>(e => e.Type.Property == testObject.Property)), Times.Once);
        }

        [Fact]
        public void WhenPublishNotificationEvent()
        {
            //Arrange
            var subject = Mocker.CreateInstance<EventPublisher>();
            var exchangeName = AutoFixture.Create<string>();
            var notificationEvent = AutoFixture.Create<NotificationEvent>();
            var routingKey = $"{notificationEvent.Notification.GetType().Name.ToLower()}Event";

            Mocker.GetMock<IProvideConfiguration>()
                .Setup(getter => getter.GetStringValue("RabbitMq:ExchangeName"))
                .Returns(exchangeName);

            //Act
            subject.Publish(notificationEvent);

            //Assert
            Mocker.Verify<IPublishRabbitMqMessage>(publisher =>
                publisher.Publish(
                    exchangeName,
                    routingKey,
                    It.Is<NotificationEvent>(e => e.Notification == notificationEvent.Notification)), Times.Once);
        }

        [Fact]
        public void WhenPublishAddEvent()
        {
            //Arrange
            var subject = Mocker.CreateInstance<EventPublisher>();
            var exchangeName = AutoFixture.Create<string>();
            var testObject = AutoFixture.Create<TestObject>();
            var addEvent = new AddEvent<TestObject>(testObject);
            var routingKey = $"{addEvent.Added.GetType().Name.ToLower()}Add";

            Mocker.GetMock<IProvideConfiguration>()
                .Setup(getter => getter.GetStringValue("RabbitMq:ExchangeName"))
                .Returns(exchangeName);

            //Act
            subject.Publish(addEvent);

            //Assert
            Mocker.Verify<IPublishRabbitMqMessage>(publisher =>
                publisher.Publish(
                    exchangeName,
                    routingKey,
                    It.Is<AddEvent<TestObject>>(e => e.Added.Property == testObject.Property)), Times.Once);
        }

        [Fact]
        public void WhenPublishUpdateEvent()
        {
            //Arrange
            var subject = Mocker.CreateInstance<EventPublisher>();
            var exchangeName = AutoFixture.Create<string>();
            var testObject = AutoFixture.Create<TestObject>();
            var updateEvent = new UpdateEvent<TestObject>(testObject);
            var routingKey = $"{updateEvent.Update.GetType().Name.ToLower()}Update";

            Mocker.GetMock<IProvideConfiguration>()
                .Setup(getter => getter.GetStringValue("RabbitMq:ExchangeName"))
                .Returns(exchangeName);

            //Act
            subject.Publish(updateEvent);

            //Assert
            Mocker.Verify<IPublishRabbitMqMessage>(publisher =>
                publisher.Publish(
                    exchangeName,
                    routingKey,
                    It.Is<UpdateEvent<TestObject>>(e => e.Update.Property == testObject.Property)), Times.Once);
        }

        [Fact]
        public void WhenPublishNotifyEvent()
        {
            //Arrange
            var subject = Mocker.CreateInstance<EventPublisher>();
            var exchangeName = AutoFixture.Create<string>();
            var testObject = AutoFixture.Create<TestObject>();
            var updateEvent = new NotifyEvent<TestObject>(testObject);
            var routingKey = $"{updateEvent.Notify.GetType().Name.ToLower()}Notify";

            Mocker.GetMock<IProvideConfiguration>()
                .Setup(getter => getter.GetStringValue("RabbitMq:ExchangeName"))
                .Returns(exchangeName);

            //Act
            subject.Publish(updateEvent);

            //Assert
            Mocker.Verify<IPublishRabbitMqMessage>(publisher =>
                publisher.Publish(
                    exchangeName,
                    routingKey,
                    It.Is<NotifyEvent<TestObject>>(e => e.Notify.Property == testObject.Property)), Times.Once);
        }
    }
}
