using Otb.Immutable;
using Otb.RabbitMq.Utilities.Messages;
using RabbitMQ.Client;

namespace RabbitMeddling.Services
{
    public class RabbitMqMessagePublisher : IPublishRabbitMqMessage
    {
        private readonly IAmABasicPropertiesFactory _basicPropertiesFactory;
        private readonly ISerializeMessages _serializeMessages;
        private readonly IModel _channel;

        public RabbitMqMessagePublisher(IAmABasicPropertiesFactory basicPropertiesFactory, ISerializeMessages serializeMessages, IModel channel)
        {
            _basicPropertiesFactory = basicPropertiesFactory;
            _serializeMessages = serializeMessages;
            _channel = channel;
        }
        public void Publish<TPayload>(string exchangeName, string routingKey, TPayload payload) where TPayload : DataContractImmutable<TPayload>
        {
            var basicProps = _basicPropertiesFactory.GetDurableJsonMessageProperties();
            var messageBytes = _serializeMessages.ToByteArray(payload);

            _channel.ExchangeDeclare(exchangeName, "topic", true, false, null);
            _channel.BasicPublish(exchangeName, routingKey, false, basicProps, messageBytes);
        }
    }
}