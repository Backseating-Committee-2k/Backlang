using Flo;
using MrKWatkins.Ast;

namespace SocordiaC.Stages;

public sealed class PrintErrorsStage : IHandler<Driver, Driver>
{
    public static List<string> Errors = [];

    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var tree in context.Trees) Errors.AddRange(MessageFormatter.FormatErrors(tree.Declarations));

        foreach (var error in Errors) Console.WriteLine(error);

        return await next.Invoke(context);
    }
}