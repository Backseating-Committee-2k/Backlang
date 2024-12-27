using System.Reflection;
using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public class CollectClassesListener : Listener<Driver, AstNode, ClassDeclaration>
{
    protected override void ListenToNode(Driver context, ClassDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name,
            GetModifiers(node), (TypeDefOrSpec)GetBaseType(node, context.Compilation));

        foreach (var baseType in node.Implementations)
        {
            var t = Utils.GetTypeFromNode(baseType, context.Compilation.Module);

            if (t.IsInterface)
            {
                type.Interfaces.Add(t);
            }
            else
            {
                baseType.AddError(baseType + " is not an interface");
            }
        }
    }

    private TypeDesc? GetBaseType(ClassDeclaration node, DistIL.Compilation compilation)
    {
        if (node.BaseType == null)
        {
            return compilation.Module.Resolver.Import(typeof(object));
        }

        return Utils.GetTypeFromNode(node.BaseType, compilation.Module);
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