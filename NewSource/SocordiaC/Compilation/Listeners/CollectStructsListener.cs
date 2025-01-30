using System.Reflection;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation.Listeners;

public class CollectStructsListener : Listener<Driver, AstNode, StructDeclaration>
{
    protected override void ListenToNode(Driver context, StructDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name,
            Utils.GetTypeModifiers(node) | TypeAttributes.Sealed | TypeAttributes.SequentialLayout,
            context.Compilation.Resolver.SysTypes.ValueType);

        Utils.EmitAnnotations(node, type);
    }
}