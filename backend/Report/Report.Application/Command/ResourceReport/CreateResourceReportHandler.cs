
using Application.Shared.Exceptions;
using Greenhouse.Contracts.Resource;
using MediatR;
using Report.Application.Common.Interfaces;
using Report.Application.Contracts.File;
using Xceed.Document.NET;
using Xceed.Words.NET;
using ResourceReportEntity = Report.Application.Contracts.ResourcesReport.ResourceReport;


namespace Report.Application.Command.ResourceReport
{
    public class CreateResourceReportHandler :
        IRequestHandler<CreateResourceReportCommad, FileResponse>
    {
        private readonly IReportService<ResourceReportEntity> reportService;

        public CreateResourceReportHandler(
            IReportService<ResourceReportEntity> reportService)
        {
            this.reportService = reportService;
        }

        public async Task<FileResponse> Handle(
            CreateResourceReportCommad request, 
            CancellationToken cancellationToken)
        {
            var resources = await reportService.GetReportData(
                new EventResourceSpendingRequest
                {
                    EventId = request.EventId,
                });

            if(resources.IsFailure)
            {
                throw new BadRequestException("Error Get Data For Report");
            }

            var stream = new MemoryStream();
            using var document = DocX.Create(stream);

            var headParagraph = document.InsertParagraph();
            headParagraph.Append($"Report Event Spending Resources From {DateTime.UtcNow:g} {request.EventId}")
                .Font("Times New Roman")
                .FontSize(14);
            headParagraph.Alignment = Alignment.center;

            var resourcesList = resources.Value.ToList();

            var table = document.AddTable(resourcesList.Count + 1, 5);

            var cell = table.Rows[0].Cells[0];
            cell.Paragraphs.First()
            .Append("Name")
                .FontSize(14)
                .Font("Times New Roman")
                .Alignment = Alignment.center;

            cell = table.Rows[0].Cells[1];
            cell.Paragraphs.First()
            .Append("Unit")
                .FontSize(14)
                .Font("Times New Roman")
                .Alignment = Alignment.center;

            cell = table.Rows[0].Cells[2];
            cell.Paragraphs.First()
            .Append("Resource Type")
                .FontSize(14)
                .Font("Times New Roman")
                .Alignment = Alignment.center;

            cell = table.Rows[0].Cells[3];
            cell.Paragraphs.First()
            .Append("Planned Amount")
                .FontSize(14)
                .Font("Times New Roman")
                .Alignment = Alignment.center;

            cell = table.Rows[0].Cells[4];
            cell.Paragraphs.First()
            .Append("Actual Amount")
                .FontSize(14)
                .Font("Times New Roman")
                .Alignment = Alignment.center;

            for(int i = 0; i < resourcesList.Count; i++)
            {
                var cellTable = table.Rows[i + 1];

                cellTable.Cells[0].Paragraphs.First()
                    .Append(resourcesList[i].Name)
                    .FontSize(14)
                    .Font("Times New Roman")
                    .Alignment = Alignment.center;

                cellTable.Cells[1].Paragraphs.First()
                    .Append(resourcesList[i].Unit)
                    .FontSize(14)
                    .Font("Times New Roman")
                    .Alignment = Alignment.center;

                cellTable.Cells[2].Paragraphs.First()
                    .Append(resourcesList[i].ResourceType)
                    .FontSize(14)
                    .Font("Times New Roman")
                    .Alignment = Alignment.center;

                cellTable.Cells[3].Paragraphs.First()
                    .Append(resourcesList[i].PlannedAmount.ToString())
                    .FontSize(14)
                    .Font("Times New Roman")
                    .Alignment = Alignment.center;

                cellTable.Cells[4].Paragraphs.First()
                    .Append(resourcesList[i].ActualAmount.ToString())
                    .FontSize(14)
                    .Font("Times New Roman")
                    .Alignment = Alignment.center;
            }

            document.InsertTable(table);

            document.Save();
            stream.Position = 0;

            return new FileResponse 
            { 
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                Stream = stream,
                FileName = $"Report ${DateTime.UtcNow:g}.docx"
            };
        }
    }
}
