
namespace Greenhouse.Contracts.Emploees
{
    public class CreateEmployeResult
    {
        public string Email { get; set; } = string.Empty;

        public string StatusText { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }
    }
}
