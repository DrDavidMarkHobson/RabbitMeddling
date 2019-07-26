using Otb.Immutable;

namespace RabbitMeddling.Services.Events
{
    public class NotifyEvent<T> : DataContractImmutable<NotifyEvent<T>>
    {
        private readonly T _notify;
        public NotifyEvent(T notify)
        {
            _notify = notify;
        }
        public T Notify => _notify;
    }
}