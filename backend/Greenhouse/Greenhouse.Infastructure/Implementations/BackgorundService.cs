using Greenhouse.Application.Common.Intrefaces;
using Hangfire;
using System.Linq.Expressions;

namespace Greenhouse.Infastructure.Implementations
{
    public class BackgorundService : IBackgroundService
    {
        private readonly IBackgroundJobClient jobClient;
        private readonly IRecurringJobManager jobManager;

        public BackgorundService(
            IBackgroundJobClient jobClient,
            IRecurringJobManager jobManager)
        {
            this.jobClient = jobClient;
            this.jobManager = jobManager;
        }

        public string Enqueue<T>(Expression<Action<T>> methodCall)
        {
            return jobClient.Enqueue<T>(methodCall);
        }

        public void Recurring<T>(string jobName, Expression<Action<T>> methodCall, string cropExpression)
        {
            jobManager.AddOrUpdate(jobName, methodCall, cropExpression);
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        {
            return jobClient.Schedule(methodCall, delay);
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, DateTime dateTime)
        {
            return jobClient.Schedule(methodCall, dateTime);
        }
    }
}
