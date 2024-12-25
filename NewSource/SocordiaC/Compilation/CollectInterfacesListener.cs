using System.Reflection;
using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public class CollectInterfacesListener : Listener<Driver, AstNode, InterfaceDeclaration>
{
    protected override void ListenToNode(Driver context, InterfaceDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name,
            GetModifiers(node) | TypeAttributes.Interface | TypeAttributes.Abstract,
            context.Compilation.Resolver.SysTypes.Object);
    }

    private TypeDefOrSpec? GetBaseType(ClassDeclaration node, DistIL.Compilation compilation)
    {
        if (node.Inheritances.Count == 0)
        {
            return compilation.Module.Resolver.Import(typeof(object));
        }

        return Utils.GetTypeFromNode(node.Inheritances[0], compilation.Module);
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