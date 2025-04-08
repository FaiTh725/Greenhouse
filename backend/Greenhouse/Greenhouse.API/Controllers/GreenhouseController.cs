using Greenhouse.Application.Commands.GreenhouseEntity.CreateGreenhouse;
using Greenhouse.Application.Queries.Greenhouse.GetGreenhouesesPagination;
using Greenhouse.Application.Queries.Greenhouse.GetGreenhouseById;
using Greenhouse.Application.Queries.Greenhouse.GetGreenhouseCompletedEvents;
using Greenhouse.Application.Queries.Greenhouse.GetGreenhouseEvents;
using Greenhouse.Application.Queries.Greenhouse.GetGreenhouseEventsByDay;
using MassTransit.InMemoryTransport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Greenhouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GreenhouseController : ControllerBase
    {
        private readonly IMediator mediator;

        public GreenhouseController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateGreenhouse(
            CreateGreenhouseCommand request)
        {
            var greenhouseId = await mediator.Send(request);

            var greenhouse = await mediator.Send(new GetGreenhouseByIdQuery
            {
                Id = greenhouseId,
            });

            return Ok(greenhouse);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetGreenhouse(long id)
        {
            var greenhouse = await mediator.Send(new GetGreenhouseByIdQuery
            {
                Id = id
            });

            return Ok(greenhouse);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetGreenhouseEvents(long id)
        {
            var greenhouseEvents = await mediator.Send(
                new GetGreenhouseEventsQuery
                {
                    GreenhouseId = id
                });

            return Ok(greenhouseEvents);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetGreenhouseCompletedEvents(long id)
        {
            var greenhouseEvents = await mediator.Send(new GetGreenhouseCompletedEventsQuery
            {
                Id = id
            });

            return Ok(greenhouseEvents);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetGreenhouseEventsByDay(
            long id, DateOnly eventsDay)
        {
            var greenhouseEvents = await mediator.Send(
                new GetGreenhouseEventsByDayQuery
                {
                    GreenhouseId = id,
                    EventsDay = eventsDay
                });

            return Ok(greenhouseEvents);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetGreenhouses(
            int page, int count)
        {
            var greenhouses = await mediator.Send(
                new GetGreenhouesesPaginationQuery
                {
                    Count = count,
                    Page = page,
                });

            return Ok(greenhouses);
        }
    }
}
