using Application.Shared.Exceptions;
using Greenhouse.API.Contracts.GreenhouseEvent;
using Greenhouse.Application.Commands.Event.AddEvent;
using Greenhouse.Application.Commands.Event.CancelEvent;
using Greenhouse.Application.Commands.Event.CompleteEvent;
using Greenhouse.Application.Commands.Event.ProcessEvent;
using Greenhouse.Application.Common.Intrefaces;
using Greenhouse.Application.Contracts.Employe;
using Greenhouse.Application.Queries.Event.GetEventByIdWithExecuter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Greenhouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GreenhouseEventController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IJwtTokenService<EmployeToken> jwtTokenService;

        public GreenhouseEventController(
            IMediator mediator,
            IJwtTokenService<EmployeToken> jwtTokenService)
        {
            this.mediator = mediator;
            this.jwtTokenService = jwtTokenService;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddGeenhouseEvent(
            AddEventCommand request)
        {
            var eventId = await mediator.Send(request);

            var eventEntity = await mediator.Send(
                new GetEventByIdWithExecuterQuery
                {
                    Id = eventId,
                });

            return Ok(eventEntity);
        }

        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> CancelGreenhouseEvent(
            CancelEventCommand request)
        {
            await mediator.Send(request);

            return Ok();
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> ProcessEvent(
            ProccessEventRequest request)
        {
            var token = Request.Cookies["token"] ??
                throw new ForbiddenAccessException("Auth failure");

            var decodeToken = jwtTokenService.DecodeToken(token);

            if(decodeToken.IsFailure)
            {
                throw new InternalServerApiException(decodeToken.Error);
            }

            await mediator.Send(new ProcessEventCommand
            {
                EventId = request.EventId,
                EmployeEmail = decodeToken.Value.Email,
                Role = decodeToken.Value.Role
            });

            return Ok();
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CompleteEvent(
            CompleteEventRequest request)
        {
            var token = Request.Cookies["token"] ??
                throw new ForbiddenAccessException("Auth failure");

            var decodeToken = jwtTokenService.DecodeToken(token);

            if (decodeToken.IsFailure)
            {
                throw new InternalServerApiException(decodeToken.Error);
            }

            await mediator.Send(new CompleteEventCommand
            {
                EmployeEmail = decodeToken.Value.Email,
                Role = decodeToken.Value.Role,
                EventId = request.EventId,
                ActualResources = request.ActualResources
            });

            return Ok();
        }
    }
}
