using Flo;
using SocordiaC.Compilation;

namespace SocordiaC.Stages;

public sealed class OptimizeStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        context.Optimizer.Run(Mappings.Functions.Values);

        return await next.Invoke(context);
    }
}