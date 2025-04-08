using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Application.Contracts.Greenhouse;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouesesPagination
{
    public class GetGreenhouesesPaginationQuery :
        IRequest<GreenhousePaginationResponse>,
        ICacheQuery
    {
        public int Page { get; set; }  

        public int Count { get; set; }

        public string Key => $"Greenhouses:{Page}-{Count}";

        public int ExpirationSeconds => 120;
    }
}
