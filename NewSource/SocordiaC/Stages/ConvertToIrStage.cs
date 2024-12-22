using Flo;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using SocordiaC.Compilation;

namespace SocordiaC.Stages;

public class ConvertToIrStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        var pipeline = CompositeListener<Driver, AstNode>.Build()
           // .With(new BlockListener())
            .With(new CollectFunctionsListener(context.FunctionsType))
            .ToListener();

        foreach (var tree in context.Trees)
        {
            foreach (var decl in tree.Declarations)
            {
                pipeline.Listen(context, decl);
            }
        }

        return await next.Invoke(context);
    }
}