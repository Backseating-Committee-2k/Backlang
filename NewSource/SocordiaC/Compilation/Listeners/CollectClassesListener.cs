using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation.Listeners;

public class CollectClassesListener : Listener<Driver, AstNode, ClassDeclaration>
{
    protected override void ListenToNode(Driver context, ClassDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name, Utils.GetTypeModifiers(node));

        Mappings.Types[node] = type;

        Utils.EmitAnnotations(node, type);
    }
}