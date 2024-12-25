using Flo;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using SocordiaC.Compilation;

namespace SocordiaC.Stages;

public class ConvertToIrStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        var pipeline = CompositeListener<Driver, AstNode>.Build()
            .With(new CollectFunctionsListener())
            .With(new CollectClassesListener())
            .With(new CollectEnumListener())
            .With(new CollectUnitsListener())
            .With(new CollectUnionsListener())
            .With(new CollectInterfacesListener())
            .ToListener();

        foreach (var tree in context.Trees)
        {
            foreach (var decl in tree.Declarations.Children)
            {
                pipeline.Listen(context, decl);
            }
        }

        return await next.Invoke(context);
    }
}