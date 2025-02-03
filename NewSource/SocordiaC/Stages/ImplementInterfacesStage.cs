using DistIL.AsmIO;
using Flo;
using Socordia.CodeAnalysis.AST.Declarations;
using SocordiaC.Compilation;

namespace SocordiaC.Stages;

public class ImplementInterfacesStage : IHandler<Driver, Driver>
{
    public async Task<Driver> HandleAsync(Driver context, Func<Driver, Task<Driver>> next)
    {
        foreach (var (node, type) in Mappings.Types)
        {
            foreach (var impl in node.Implementations)
            {
                var t = Utils.GetTypeFromNode(impl, type);

                if (t.IsInterface)
                    type.Interfaces.Add(t);
                else
                    impl.AddError(impl + " is not an interface");
            }

            type.SetBaseType((TypeDefOrSpec)GetBaseType(node, context.Compilation));
        }

        Mappings.Types.Clear();

        return await next.Invoke(context);
    }

    private static TypeDesc GetBaseType(ClassDeclaration node, DistIL.Compilation compilation)
    {
        if (node.BaseType == null) return compilation.Module.Resolver.Import(typeof(object));

        return Utils.GetTypeFromNode(node.BaseType, compilation.Module)!;
    }
}