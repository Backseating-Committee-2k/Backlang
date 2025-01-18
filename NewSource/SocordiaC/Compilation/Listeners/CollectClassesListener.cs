using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation.Listeners;

public class CollectClassesListener : Listener<Driver, AstNode, ClassDeclaration>
{
    protected override void ListenToNode(Driver context, ClassDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name,
            Utils.GetTypeModifiers(node), (TypeDefOrSpec)GetBaseType(node, context.Compilation));

        Mappings.Types[node] = type;

        Utils.EmitAnnotations(node, type);
    }

    private TypeDesc? GetBaseType(ClassDeclaration node, DistIL.Compilation compilation)
    {
        if (node.BaseType == null) return compilation.Module.Resolver.Import(typeof(object));

        return Utils.GetTypeFromNode(node.BaseType, compilation.Module);
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node)
    {
        return true;
    }
}