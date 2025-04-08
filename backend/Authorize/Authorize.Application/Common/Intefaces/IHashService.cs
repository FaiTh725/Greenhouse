namespace Authorize.Application.Common.Intefaces
{
    public interface IHashService
    {
        public string GenerateHash(string inputStr);

        public bool VerifyHash(string inputStr, string hash);
    }
}
