using Otb.Immutable;

namespace RabbitMeddling.Services.Events
{
    public class Event<T> : DataContractImmutable<Event<T>>
    {
        private readonly T _event;
        private readonly string _name;
        public Event(T eventInstance, string name)
        {
            _event = eventInstance;
            _name = name;
        }
        public T Type => _event;
    }
}