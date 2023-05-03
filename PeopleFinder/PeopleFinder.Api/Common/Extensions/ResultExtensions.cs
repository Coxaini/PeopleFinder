using FluentResults;

namespace PeopleFinder.Api.Common.Extensions
{
    public static class ResultExtensions
    {
        public static TResult Match<TResult, TValue> (this Result<TValue> result,
            Func<TValue, TResult> onValue, Func<List<IError>, TResult> onError)
        {
            if (result.IsFailed)
            {
                return onError(result.Errors);
            }
            return onValue(result.Value);
        }
        public static TResult Match<TResult> (this Result result,
            Func<TResult> onOk, Func<List<IError>, TResult> onError)
        {
            if (result.IsFailed)
            {
                return onError(result.Errors);
            }
            return onOk();
        }

        

        

    }
}
