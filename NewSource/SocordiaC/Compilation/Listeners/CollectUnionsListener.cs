using System.Reflection;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.Compilation;

namespace SocordiaC.Compilation.Listeners;

public class CollectUnionsListener : Listener<Driver, AstNode, UnionDeclaration>
{
    protected override void ListenToNode(Driver context, UnionDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name,
            GetModifiers(node) | TypeAttributes.ExplicitLayout | TypeAttributes.BeforeFieldInit,
            context.Compilation.Resolver.SysTypes.Object);

        type.AddCompilerGeneratedAttribute(context.Compilation);

        Utils.EmitAnnotations(node, type);
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
            attrs &= ~TypeAttributes.Public;

        return attrs;
    }
}