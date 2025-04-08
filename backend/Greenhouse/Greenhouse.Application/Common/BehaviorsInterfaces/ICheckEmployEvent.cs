namespace Greenhouse.Application.Common.BehaviorsInterfaces
{
    public interface ICheckEmployEvent
    {
        public long EventId { get; }

        public string EmployeEmail { get; }

        public string Role { get; }
    }
}
