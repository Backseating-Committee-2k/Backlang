using System.Reflection;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public class CollectClassesListener : Listener<Driver, AstNode, ClassDeclaration>
{
    protected override void ListenToNode(Driver context, ClassDeclaration node)
    {
        var type = context.Compilation.Module.CreateType(context.Settings.RootNamespace, node.Name, TypeAttributes.Public);
        
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => true;
}