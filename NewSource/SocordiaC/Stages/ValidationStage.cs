using Flo;
using Socordia.CodeAnalysis.Validation;

namespace SocordiaC.Stages;

public sealed class ValidationStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var tree in context.Trees)
        {
            NodeValidator.Pipeline.Run(tree.Declarations);
        }

        return await next.Invoke(context);
    }
}