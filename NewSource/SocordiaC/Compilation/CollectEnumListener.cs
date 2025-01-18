using System.Reflection;
using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public class CollectEnumListener : Listener<Driver, AstNode, EnumDeclaration>
{
    protected override void ListenToNode(Driver context, EnumDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name,
            GetModifiers(node), context.Compilation.Module.Resolver.Import(typeof(Enum)));

        type.CreateField("value__", new TypeSig(Utils.GetTypeFromNode(node.BaseType, type)), FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);

        // .field public static literal valuetype Color R = int32(0)
        foreach (var astNode in node.Children)
        {
            var member = (EnumMemberDeclaration)astNode;

            type.CreateField(member.Name.Name, new TypeSig(type),
                FieldAttributes.Public | FieldAttributes.Literal | FieldAttributes.Static | FieldAttributes.HasDefault,
                Utils.GetLiteralValue(member.Value));
        }

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
        {
            attrs &= ~TypeAttributes.Public;
        }

        return attrs | TypeAttributes.Sealed;
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => true;
}