using Flo;
using System.Diagnostics;

namespace SocordiaC.Stages;

public sealed class WaitForDebuggerStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        Console.WriteLine("Waiting for debugger to attach...");
        while (!Debugger.IsAttached)
        {
            Thread.Sleep(1);
        }

        Console.WriteLine("Debugger attached");

        return await next.Invoke(context);
    }
}