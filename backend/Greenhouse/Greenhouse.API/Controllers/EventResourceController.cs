using Greenhouse.Application.Commands.EventResource.AddEventResource;
using Greenhouse.Application.Queries.EventResource.GetEventResources;
using Greenhouse.Application.Queries.EventResource.GetEventWithResById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Greenhouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventResourceController : ControllerBase
    {
        private readonly IMediator mediator;

        public EventResourceController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddEventResource(
            AddEventResourceCommand request)
        {
            var eventResourceId = await mediator.Send(request);

            var eventResource = await mediator.Send(new GetEventWithResByIdQuery
            {
                EventResourceId = eventResourceId
            });

            return Ok(eventResource);
        }

        [HttpGet]
        public async Task<IActionResult> GetEventResources(
            long eventId)
        {
            var eventResources = await mediator.Send(
                new GetEventResourcesQuery 
                { 
                    EventId = eventId
                });

            return Ok(eventResources);
        }
    }
}
