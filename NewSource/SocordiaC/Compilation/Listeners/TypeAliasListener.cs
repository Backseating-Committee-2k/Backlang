using MrWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;

namespace SocordiaC.Compilation;

public class TypeAliasListener : Listener<Driver, AstNode, TypeAliasDeclaration>
{
    protected override void ListenToNode(Driver context, TypeAliasDeclaration node)
    {
        var root = (RootBlock)node.Root.Scope;

        root.Scope.TypeAliases.Add(node.Name, node.Type);
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, TypeAliasDeclaration node) => false;
}