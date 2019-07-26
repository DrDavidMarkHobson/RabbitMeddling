using Otb.Immutable;

namespace RabbitMeddling.Services.Events
{
    public class AddEvent<T> : DataContractImmutable<AddEvent<T>>
    {
        private readonly T _added;
        public AddEvent(T added)
        {
            _added = added;
        }
        public T Added => _added;
    }
}