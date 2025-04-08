using CSharpFunctionalExtensions;

namespace Greenhouse.Application.Common.Intrefaces
{
    public interface ICacheService
    {
        Task<Result<T>> GetData<T>(string key);

        Task SetData<T>(string key, T value, int expirationSecond);

        Task RemoveData(string key);

        Task RemoveByPatern(string pattern);
    }
}
