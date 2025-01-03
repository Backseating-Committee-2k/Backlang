using Flo;
using SocordiaC.Compilation.Body.Lowering;

namespace SocordiaC.Stages;

public sealed class LoweringStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var tree in context.Trees)
        {
            Lowerer.Pipeline.Run(tree.Declarations);
        }

        return await next.Invoke(context);
    }
}