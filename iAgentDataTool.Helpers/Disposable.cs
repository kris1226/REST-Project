using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System {
    public static class Disposable {
        public static async Task<TResult> UsingAsync<TDisposable, TResult>(Func<TDisposable> factory, Func<TDisposable, Task<TResult>> map)
            where TDisposable : IDisposable {
                using (var disposable = factory()) {
                    return await map(disposable);
                }
        }
        public static async Task UsingVoid<TDisposable>(Func<TDisposable> factory, Func<TDisposable, Task> map)
           where TDisposable : IDisposable {
            using (var disposable = factory()) {
                await map(disposable);
            }
        }
    }
}
