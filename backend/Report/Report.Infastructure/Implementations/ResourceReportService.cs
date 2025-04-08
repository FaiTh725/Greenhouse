using CSharpFunctionalExtensions;
using Greenhouse.Contracts.Resource;
using MassTransit;
using Report.Application.Common.Interfaces;
using Report.Application.Contracts.ResourcesReport;

namespace Report.Infastructure.Implementations
{
    public class ResourceReportService :
        IReportService<ResourceReport>
    {
        private readonly IRequestClient<EventResourceSpendingRequest> client;

        public ResourceReportService(
            IRequestClient<EventResourceSpendingRequest> client)
        {
            this.client = client;
        }

        public async Task<Result<IEnumerable<ResourceReport>>> GetReportData<TRequestData>(
            TRequestData requestData)
        {
            if(requestData is null)
            {
                return Result.Failure<IEnumerable<ResourceReport>>("Request Data is null");
            }

            var resources = await client
                .GetResponse<EventResourceSpendingResponse>(requestData);

            if(!resources.Message.IsSuccess)
            {
                return Result.Failure<IEnumerable<ResourceReport>>(resources.Message.Message);
            }

            return Result.Success(resources.Message.Resources
                .Select(x => new ResourceReport
                {
                    ResourceId = x.Id,
                    Name = x.Name,
                    Unit = x.Unit,
                    ActualAmount = x.ActualAmount.Value,
                    PlannedAmount = x.PlannedAmount,
                    ResourceType = x.ResourceType
                }).AsEnumerable());
        }
    }
}
