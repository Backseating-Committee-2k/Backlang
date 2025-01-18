using Flo;
using SocordiaC.Compilation;

namespace SocordiaC.Stages;

public sealed class OptimizeStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        var passManager = context.Optimizer.PassManager;

        passManager.Run(Mappings.Functions.Values);

        return await next.Invoke(context);
    }
}