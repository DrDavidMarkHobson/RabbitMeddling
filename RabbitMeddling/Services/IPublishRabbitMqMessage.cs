using Otb.Immutable;

namespace RabbitMeddling.Services
{
    public interface IPublishRabbitMqMessage
    {
        void Publish<TPayload>(string exchangeName, string routingKey, TPayload payload)
            where TPayload : DataContractImmutable<TPayload>;
    }
}