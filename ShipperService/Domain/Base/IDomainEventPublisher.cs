using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.Domain
{
    public interface IDomainEventPublisher
    {
        Task Publish(DomainEvent @event);
        Task Publish(IEnumerable<DomainEvent> events);
    }
}
