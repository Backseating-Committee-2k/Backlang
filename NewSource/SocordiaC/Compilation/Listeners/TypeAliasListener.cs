using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.TypeNames;
using SocordiaC.Compilation.Scoping;

namespace SocordiaC.Compilation.Listeners;

public class TypeAliasListener : Listener<Driver, AstNode, TypeAliasDeclaration>
{
    protected override void ListenToNode(Driver context, TypeAliasDeclaration node)
    {
        var root = (Scope)node.Root.Tag!;

        root.TypeAliases.Add(node.Name.Name, new SimpleTypeName((Identifier)node.Type));
    }

    protected override bool ShouldListenToChildren(Driver context, TypeAliasDeclaration node) => false;
}