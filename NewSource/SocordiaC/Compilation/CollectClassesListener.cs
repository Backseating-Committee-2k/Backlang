using System.Reflection;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public class CollectClassesListener : Listener<Driver, AstNode, ClassDeclaration>
{
    protected override void ListenToNode(Driver context, ClassDeclaration node)
    {
        var type = context.Compilation.Module.CreateType(context.Settings.RootNamespace, node.Name,
            GetModifiers(node));

    }

    private TypeAttributes GetModifiers(Declaration node)
    {
        var attrs = TypeAttributes.Public;

        foreach (var modifier in node.Modifiers)
        {
            attrs |= modifier switch
            {
                Modifier.Static => TypeAttributes.Sealed | TypeAttributes.Abstract,
                Modifier.Internal => TypeAttributes.NotPublic,
                Modifier.Public => TypeAttributes.Public,
                _ => throw new NotImplementedException()
            };
        }

        if (node.Modifiers.Contains(Modifier.Private) || node.Modifiers.Contains(Modifier.Internal))
        {
            attrs &= ~TypeAttributes.Public;
        }

        return attrs;
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => true;
}