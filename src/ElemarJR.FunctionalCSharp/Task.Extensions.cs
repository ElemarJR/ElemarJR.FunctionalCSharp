using System;
using System.Threading.Tasks;

namespace ElemarJR.FunctionalCSharp
{
    public static class TaskExtensions
    {
        public static async Task<TResult> Map<TInput, TResult>(
            this Task<TInput> task,
            Func<TInput, TResult> func
        ) => func(await task);

        public static Task<TResult> Map<TInput, TResult>(
            this Task<TInput> task,
            Func<Exception, TResult> failure,
            Func<TInput, TResult> success
        ) => task.ContinueWith(t => t.Status == TaskStatus.Faulted
            ? failure(t.Exception)
            : success(t.Result)
            );
        public static async Task<TResult> Bind<TInput, TResult>(
            this Task<TInput> task,
            Func<TInput, Task<TResult>> func
        ) => await func(await task);

        public static Task<T> OrElse<T>(
            this Task<T> task,
            Func<Task<T>> fallback
        ) => task.ContinueWith(t =>
            t.Status == TaskStatus.Faulted
            ? fallback()
            : Task.FromResult(t.Result)
        ).Unwrap();

    }
}
