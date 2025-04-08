using CSharpFunctionalExtensions;

namespace Report.Application.Common.Interfaces
{
    public interface IReportService<ReportData>
    {
        Task<Result<IEnumerable<ReportData>>> GetReportData<TRequestData>(TRequestData requestData);
    }
}
