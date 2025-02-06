using Flo;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using SocordiaC.Compilation.Listeners;

namespace SocordiaC.Stages;

public class CollectTypesStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        var collectTypesPipeline = CompositeListener<Driver, AstNode>.Build()
            .With(new CollectClassesListener())
            .With(new CollectStructsListener())
            .With(new CollectEnumListener())
            .With(new CollectUnitsListener())
            .With(new CollectUnionsListener())
            .With(new CollectInterfacesListener())
            .With(new CollectDUsListener())
            .ToListener();

        foreach (var tree in context.Trees)
        foreach (var decl in tree.Declarations.Children)
        {
            collectTypesPipeline.Listen(context, decl);
        }

        var functionCollector = new CollectFunctionsListener();
        foreach (var tree in context.Trees)
        foreach (var decl in tree.Declarations.Children)
        {
            functionCollector.Listen(context, decl);
        }

        return await next.Invoke(context);
    }
}