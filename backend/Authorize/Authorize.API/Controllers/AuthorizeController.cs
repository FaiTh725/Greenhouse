using Authorize.Application.Commands.User.AcceptEmail;
using Authorize.Application.Commands.User.ActiveUsers;
using Authorize.Application.Commands.User.ConfirmUserEmail;
using Authorize.Application.Commands.User.LoginUser;
using Authorize.Application.Commands.User.RegisterUser;
using Authorize.Application.Common.Intefaces;
using Authorize.Application.Contracts.JwtToken;
using Authorize.Application.Contracts.User;
using Authorize.Application.Queries.User.GetUnconfirmedUsers;
using Authorize.Application.Queries.User.GetUserById;
using Authorize.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorize.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IJwtService<UserResponse, TokenResponse> jwtService;

        public AuthorizeController(
            IMediator mediator,
            IJwtService<UserResponse, TokenResponse> jwtService)
        {
            this.mediator = mediator;
            this.jwtService = jwtService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ConfirmEmail(
            ConfirmUserEmailCommand request)
        {
            await mediator.Send(request);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AcceptEmailConfirmation(
            AcceptEmailCommand request)
        {
            await mediator.Send(request);

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(
            [FromQuery]LoginUserCommand request)
        {
            var loginUserId = await mediator.Send(request);

            var user = await mediator.Send(new GetUserByIdQuery
            {
                Id = loginUserId,
            });

            var accessToken = jwtService.GenerateToken(user);

            var cookieOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true,
            };

            Response.Cookies.Append("token", accessToken, cookieOptions);

            return Ok(user);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(
            RegisterUserCommand request)
        {
            var userId = await mediator.Send(request);

            var user = await mediator.Send(new GetUserByIdQuery
            {
                Id = userId,
            });

            var accessToken = jwtService.GenerateToken(user);

            var cookieOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true,
            };

            Response.Cookies.Append("token", accessToken, cookieOptions);

            return Ok(user);
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveUser(
            ActiveUsersCommand request)
        {
            await mediator.Send(request);

            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUnConfirmedUsers()
        {
            var users = await mediator
                .Send(new GetUnconfirmedUsersQuery());
        
            return Ok(users);
        }
    }
}
