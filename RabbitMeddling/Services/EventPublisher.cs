using System.Collections.Generic;
using Otb.Configuration;
using RabbitMeddling.Services.Events;

namespace RabbitMeddling.Services
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IProvideConfiguration _configurationProvider;
        private readonly IPublishRabbitMqMessage _rabbitMqMessagePublisher;
        private const string ExchangeNameConfig = "RabbitMq:ExchangeName";

        public EventPublisher(
            IProvideConfiguration configurationProvider,
            IPublishRabbitMqMessage rabbitMqMessagePublisher)
        {
            _configurationProvider = configurationProvider;
            _rabbitMqMessagePublisher = rabbitMqMessagePublisher;
        }

        public void Publish<T>(Event<T> message)
        {
            var exchangeName = _configurationProvider.GetStringValue(ExchangeNameConfig);
            _rabbitMqMessagePublisher.Publish(
                exchangeName,
                $"{message.Type.GetType().Name.ToLower()}Event",
                message);
        }

        public void Publish(NotificationEvent message)
        {
            var exchangeName = _configurationProvider.GetStringValue(ExchangeNameConfig);
            _rabbitMqMessagePublisher.Publish(
                exchangeName, 
                $"{message.Notification.GetType().Name.ToLower()}Event", 
                message);
        }

        public void Publish<T>(AddEvent<T> message)
        {
            var exchangeName = _configurationProvider.GetStringValue(ExchangeNameConfig);
            _rabbitMqMessagePublisher.Publish(
                exchangeName, 
                $"{message.Added.GetType().Name.ToLower()}Add", 
                message);
        }

        public void Publish<T>(UpdateEvent<T> message)
        {
            var exchangeName = _configurationProvider.GetStringValue(ExchangeNameConfig);
            _rabbitMqMessagePublisher.Publish(
                exchangeName, 
                $"{message.Update.GetType().Name.ToLower()}Update", 
                message);
        }

        public void Publish<T>(NotifyEvent<T> message)
        {
            var exchangeName = _configurationProvider.GetStringValue(ExchangeNameConfig);
            _rabbitMqMessagePublisher.Publish(
                exchangeName, 
                $"{message.Notify.GetType().Name.ToLower()}Notify", 
                message);
        }
    }
}
