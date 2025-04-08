using MassTransit;

namespace Greenhouse.Application.Common.BackgroundWrappers
{
    public class BusWrapper
    {
        private readonly IBus bus;

        public BusWrapper(IBus bus)
        {
            this.bus = bus;
        }

        public Task Publish<T>(T request) 
            where T: class
        {
            return bus.Publish(request);
        }
    }
}
