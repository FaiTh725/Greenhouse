namespace Greenhouse.Application.Common.BehaviorsInterfaces
{
    public interface ICacheQuery
    {
        string Key { get; }

        int ExpirationSeconds { get; }
    }
}
