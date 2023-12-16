using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSystemWebTest.Async
{
    public static class TestAsyncExtensions
    {
        //public static IReturnsResult<TMock> ReturnsAsync<TMock, TResult>(this IReturns<TMock, Task<TResult>> setup, TResult result)
        //    where TMock : class
        //{
        //    return setup.Returns(Task.FromResult(result));
        //}

        //public static IReturnsResult<TMock> ReturnsAsync<TMock, TResult>(this IReturns<TMock, Task<TResult>> setup, TResult result, TimeSpan delay)
        //    where TMock : class
        //{
        //    return setup.Returns(() => Task.Delay(delay).ContinueWith(t => result));
        //}
    }

}
