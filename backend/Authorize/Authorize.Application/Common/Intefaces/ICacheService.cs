using CSharpFunctionalExtensions;

namespace Authorize.Application.Common.Intefaces
{
    public interface ICacheService
    {
        Task SetData<T>(string key, T value, int expiratingTime);

        Task RemoveData(string key);

        Task<Result<T>> GetData<T>(string key);
    }
}
