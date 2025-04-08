using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Application.Common.Intrefaces;
using MediatR;

namespace Greenhouse.Application.Behaviors.Cache
{
    public class CacheBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICacheQuery
    {
        private readonly ICacheService cacheService;

        public CacheBehavior(
            ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            var cacheData = await cacheService
                .GetData<TResponse>(request.Key);

            if(cacheData.IsSuccess)
            {
                return cacheData.Value;
            }

            var response = await next();

            await cacheService.SetData(
                request.Key,
                response,
                request.ExpirationSeconds);

            return response;
        }
    }
}
