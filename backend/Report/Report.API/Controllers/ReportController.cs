using Greenhouse.Contracts.Resource;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Report.Application.Command.ResourceReport;

namespace Report.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReportController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployePerfomance()
        {
            return Ok();
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSpendingResource(long eventId)
        {
            var report = await mediator.Send(new CreateResourceReportCommad
            {
                EventId = eventId
            });

            return File(report.Stream, report.ContentType, report.FileName);
        }
    }
}
