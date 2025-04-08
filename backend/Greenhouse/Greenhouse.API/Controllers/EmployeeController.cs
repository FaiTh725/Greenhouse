using Application.Shared.Exceptions;
using Greenhouse.Application.Common.Intrefaces;
using Greenhouse.Application.Contracts.Employe;
using Greenhouse.Application.Queries.Employee.GetEmplyes;
using Greenhouse.Application.Queries.Event.GetEventOfEmploye;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Greenhouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IJwtTokenService<EmployeToken> jwtService;

        public EmployeeController(
            IMediator mediator,
            IJwtTokenService<EmployeToken> jwtService)
        {
            this.mediator = mediator;
            this.jwtService = jwtService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEmployes()
        {
            var employes = await mediator.Send(new GetEmplyesQuery());

            return Ok(employes);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEmployeEvents()
        {
            var token = Request.Cookies["token"] ??
                throw new ForbiddenAccessException("Auth Failure");

            var employeToken = jwtService.DecodeToken(token);

            if (employeToken.IsFailure)
            {
                throw new InternalServerApiException("Invalid Token");
            }

            var events = await mediator.Send(new GetEventOfEmployeQuery 
            { 
                EmployeEmail = employeToken.Value.Email
            });

            return Ok(events);
        }
    }
}
