using Flo;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using SocordiaC.Compilation;
using SocordiaC.Compilation.Listeners;

namespace SocordiaC.Stages;

public class CollectTypesStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        var prePhase = CompositeListener<Driver, AstNode>.Build()
            .With(new TypeAliasListener())
            .ToListener();

        var firstPhasePipeline = CompositeListener<Driver, AstNode>.Build()
            .With(new CollectClassesListener())
            .With(new CollectStructsListener())
            .With(new CollectEnumListener())
            .With(new CollectUnitsListener())
            .With(new CollectUnionsListener())
            .With(new CollectInterfacesListener())
            .With(new CollectDUsListener())
            .ToListener();

        foreach (var tree in context.Trees)
        {
            foreach (var decl in tree.Declarations.Children)
            {
                prePhase.Listen(context, decl);
            }
        }

        foreach (var tree in context.Trees)
        {
            foreach (var decl in tree.Declarations.Children)
            {
                firstPhasePipeline.Listen(context, decl);
            }
        }

        var secondPhasePipeline = CompositeListener<Driver, AstNode>.Build()
            .With(new CollectFunctionsListener())
            .With(new CollectGlobalVariablesListener())
            .ToListener();

        foreach (var tree in context.Trees)
        {
            foreach (var decl in tree.Declarations.Children)
            {
                secondPhasePipeline.Listen(context, decl);
            }
        }

        return await next.Invoke(context);
    }
}