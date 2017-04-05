using System.Threading.Tasks;
using ElemarJR.FunctionalCSharp;

namespace System.Linq
{
    public static partial class LinqExtensions
    {
        public static async Task<TResult> Select<TInput, TResult>(
            this Task<TInput> task,
            Func<TInput, TResult> func
        ) => await task.Map(func);

        public static async Task<NewT> SelectMany<T, R, NewT>(
            this Task<T> task,
            Func<T, Task<R>> bind,
            Func<T, R, NewT> projector
        ) 
        {
            var t = await task;
            var r = await bind(t);
            return projector(t,  r);
        }
    }

}
