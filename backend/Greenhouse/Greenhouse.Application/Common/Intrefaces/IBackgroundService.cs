using System.Linq.Expressions;

namespace Greenhouse.Application.Common.Intrefaces
{
    public interface IBackgroundService
    {
        string Enqueue<T>(Expression<Action<T>> methodCall);

        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);

        string Schedule<T>(Expression<Action<T>> methodCall, DateTime dateTime);

        void Recurring<T>(string jobName, Expression<Action<T>> methodCall, string cropExpression);
    }
}
