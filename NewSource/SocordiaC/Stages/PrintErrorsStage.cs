using Flo;
using MrKWatkins.Ast;

namespace SocordiaC.Stages;

public sealed class PrintErrorsStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        var nodesWithError = context.Trees.SelectMany(_ => _.Declarations.ThisAndDescendentsWithErrors).ToArray();
        foreach (var tree in nodesWithError)
        {
            Console.WriteLine(string.Join('\n', MessageFormatter.FormatErrors(tree)));
        }

        return await next.Invoke(context);
    }
}