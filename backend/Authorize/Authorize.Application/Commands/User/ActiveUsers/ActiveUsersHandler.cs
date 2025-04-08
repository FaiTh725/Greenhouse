using Application.Shared.Exceptions;
using Authorize.Domain.Repositories;
using Greenhouse.Contracts.Emploees;
using MassTransit;
using MediatR;

namespace Authorize.Application.Commands.User.ActiveUsers
{
    public class ActiveUsersHandler :
        IRequestHandler<ActiveUsersCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRequestClient<CreateEmploee> requestClient;

        public ActiveUsersHandler(
            IUnitOfWork unitOfWork,
            IRequestClient<CreateEmploee> requestClient)
        {
            this.unitOfWork = unitOfWork;
            this.requestClient = requestClient;
        }

        public async Task Handle(ActiveUsersCommand request, CancellationToken cancellationToken)
        {
            var getUserTasks = request.UserEmails
                .Select(x => unitOfWork.UserRepository
                    .GetUser(x))
                .ToList();

            var users = await Task.WhenAll(getUserTasks);

            if (users.Any(x => x is null))
            {
                throw new BadRequestException("Any emails doesnt exist");
            }

            var createEmployesTasks = users.Select(x => requestClient
            .GetResponse<CreateEmployeResult>(new CreateEmploee
            {
                Email = x!.Email,
                Name = x.Name
            }));

            var resultCreateEmploes = await Task.WhenAll(createEmployesTasks);

            if (resultCreateEmploes.Any(x => !x.Message.IsSuccess))
            {
                // отправить компинсирующие действие
                throw new InternalServerApiException("Unknown server error");
            }

            foreach (var user in users)
            {
                user!.Active();
            }

            await unitOfWork.UserRepository
                .ActiveUsers(users.ToList()!);
        }
    }
}
