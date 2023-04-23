using Base.Domain;
using System;

namespace Base.Application
{
    public abstract class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
        

        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public string EntityType { get; protected set; }
    }

    public abstract class IntegrationEvent<T> : IntegrationEvent where T : IEntity
    {
        public IntegrationEvent(T entity) : base()
        {
            EntityType = entity.GetType().FullName;
        }
    }
}
