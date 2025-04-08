using MediatR;

namespace Greenhouse.Application.Commands.Event.CancelEvent
{
    public class CancelEventCommand : IRequest
    {
        public List<long> IdList { get; set; } = new List<long>();
    }
}
