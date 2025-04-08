using CSharpFunctionalExtensions;

namespace Authorize.Domain.Primitives
{
    public abstract class DomainEventEntity : Entity
    {
        private List<IDomainEvent> events = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> Events => events.ToList();

        public void ClearEvents()
        {
            events.Clear();
        }

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            events.Add(domainEvent);
        }
    }
}
