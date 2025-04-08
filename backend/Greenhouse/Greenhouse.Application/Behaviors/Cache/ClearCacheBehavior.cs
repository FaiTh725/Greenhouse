using Greenhouse.Application.Common.BehaviorsInterfaces;
using Greenhouse.Application.Common.Intrefaces;
using MediatR;

namespace Greenhouse.Application.Behaviors.Cache
{
    public class ClearCacheBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IClearCacheBehavior
    {
        private readonly ICacheService cacheService;

        public ClearCacheBehavior(
            ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            await cacheService.RemoveByPatern(request.Key);

            return response;
        }
    }
}
