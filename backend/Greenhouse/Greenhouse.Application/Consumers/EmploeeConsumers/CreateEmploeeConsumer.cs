using Greenhouse.Contracts.Emploees;
using Greenhouse.Domain.Entities;
using Greenhouse.Domain.Repositorires;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Greenhouse.Application.Consumers.EmploeeConsumers
{
    public class CreateEmploeeConsumer :
        IConsumer<CreateEmploee>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateEmploeeConsumer(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<CreateEmploee> context)
        {
            var employe = Employee.Initialize(
                context.Message.Email,
                context.Message.Name);

            if (employe.IsFailure)
            {
                await context.RespondAsync(new CreateEmployeResult
                {
                    Email = context.Message.Email,
                    IsSuccess = false,
                    StatusText = "Error initialize emploee " +
                    employe.Error
                });
                return;
            }

            await unitOfWork.EmployeRepository
                .AddEmploye(employe.Value);

            await unitOfWork.SaveChangesAsync();

            await context.RespondAsync(new CreateEmployeResult
            {
                Email = context.Message.Email,
                IsSuccess = true,
                StatusText = string.Empty
            });
        }
    }
}
