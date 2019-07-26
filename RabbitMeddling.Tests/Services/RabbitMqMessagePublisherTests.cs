using AutoFixture;
using Moq;
using Moq.AutoMock;
using Otb.Immutable;
using Otb.RabbitMq.Utilities.Messages;
using RabbitMeddling.Services;
using RabbitMQ.Client;
using Xunit;

namespace RabbitMeddling.Tests.Services
{
    public class RabbitMqMessagePublisherTests
    {
        public Fixture AutoFixture { get; set; }
        public AutoMocker Mocker { get; set; }

        public RabbitMqMessagePublisherTests()
        {
            AutoFixture = new Fixture();
            Mocker = new AutoMocker();
        }
        private class TestPayload : DataContractImmutable<TestPayload> { }
        
        [Fact]
        public void WhenPublish()
        {
            // Arrange
            var subject = Mocker.CreateInstance<RabbitMqMessagePublisher>();
            var exchangeName = AutoFixture.Create<string>();
            var routingKey = AutoFixture.Create<string>();
            var payload = AutoFixture.Create<TestPayload>();
            var basicProperties = new Mock<IBasicProperties>().Object;
            var messageBytes = AutoFixture.Create<byte[]>();
            Mocker.GetMock<IAmABasicPropertiesFactory>()
                .Setup(bpf => bpf.GetDurableJsonMessageProperties())
                .Returns(basicProperties);
            Mocker.GetMock<ISerializeMessages>().Setup(sm => sm.ToByteArray(payload)).Returns(messageBytes);

            // Act
            subject.Publish(exchangeName, routingKey, payload);

            // Assert
            Mocker.GetMock<IModel>().Verify(channel => channel.ExchangeDeclare(exchangeName, "topic", true, false, null), Times.Once);
            Mocker.GetMock<IModel>().Verify(channel => channel.BasicPublish(exchangeName, routingKey, false, basicProperties, messageBytes), Times.Once);
        }
    }
}