using Authorize.Domain.Primitives;

namespace Authorize.Domain.Events
{
    public class ActiveUserEvent : IDomainEvent
    {
        public string UserEmail {  get; set; } = string.Empty;
    }
}
