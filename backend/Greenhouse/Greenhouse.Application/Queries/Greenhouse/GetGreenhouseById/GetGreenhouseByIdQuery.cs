using Greenhouse.Application.Contracts.Greenhouse;
using MediatR;

namespace Greenhouse.Application.Queries.Greenhouse.GetGreenhouseById
{
    public class GetGreenhouseByIdQuery : 
        IRequest<GreenhouseResponse>
    {
        public long Id { get; set; }
    }
}
