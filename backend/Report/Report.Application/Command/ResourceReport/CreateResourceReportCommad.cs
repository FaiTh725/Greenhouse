using MediatR;
using Report.Application.Contracts.File;

namespace Report.Application.Command.ResourceReport
{
    public class CreateResourceReportCommad : 
        IRequest<FileResponse>
    {
        public long EventId { get; set; }
    }
}
