using Otb.Immutable;

namespace RabbitMeddling.Services.Events
{
    public class UpdateEvent<T> : DataContractImmutable<UpdateEvent<T>>
    {
        private readonly T _update;
        public UpdateEvent(T update)
        {
            _update = update;
        }
        public T Update => _update;
    }
}