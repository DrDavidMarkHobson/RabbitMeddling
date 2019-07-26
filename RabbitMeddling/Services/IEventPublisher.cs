using RabbitMeddling.Services.Events;

namespace RabbitMeddling.Services
{
    public interface IEventPublisher
    {
        void Publish(NotificationEvent message);
        void Publish<T>(Event<T> message);
        void Publish<T>(AddEvent<T> message);
        void Publish<T>(UpdateEvent<T> message);
        void Publish<T>(NotifyEvent<T> message);
    }
}