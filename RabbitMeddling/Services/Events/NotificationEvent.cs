using Otb.Immutable;

namespace RabbitMeddling.Services.Events
{
    public class NotificationEvent : DataContractImmutable<NotificationEvent>
    {
        private readonly string _notification;
        public NotificationEvent(string notification)
        {
            _notification = notification;
        }
        public string Notification => _notification;
    }
}