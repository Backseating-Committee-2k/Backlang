using System.Reflection;
using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.Literals;

namespace SocordiaC.Compilation.Listeners;

public class CollectEnumListener : Listener<Driver, AstNode, EnumDeclaration>
{
    protected override void ListenToNode(Driver context, EnumDeclaration node)
    {
        var ns = context.GetNamespaceOf(node);
        var type = context.Compilation.Module.CreateType(ns, node.Name,
            GetModifiers(node), context.Compilation.Module.Resolver.Import(typeof(Enum)));

        var valueType = Utils.GetTypeFromNode(node.BaseType, type);
        if (!valueType.IsInt())
        {
            node.BaseType.AddError("Enum base type must be an integer or char type");
            return;
        }

        type.CreateField("value__", new TypeSig(valueType),
            FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);

        if (!ValidateEnumMembers(node))
        {
            return;
        }

        // .field public static literal valuetype Color R = int32(0)
        for (int memberIndex = 0; memberIndex < node.Children.Count; memberIndex++)
        {
            if (node.Children[memberIndex] is not EnumMemberDeclaration)
            {
                continue;
            }

            var member = (EnumMemberDeclaration)node.Children[memberIndex];

             if (member.Value is EmptyNode)
            {
                member.Value.ReplaceWith(new LiteralNode(memberIndex));
            }

            type.CreateField(member.Name.Name, new TypeSig(type),
                FieldAttributes.Public | FieldAttributes.Literal | FieldAttributes.Static | FieldAttributes.HasDefault,
                Utils.GetLiteralValue(member.Value));
        }

        Utils.EmitAnnotations(node, type);
    }

    private bool ValidateEnumMembers(EnumDeclaration node)
    {
        bool hasSpecifiedValues = false;
        bool hasUnspecifiedValues = false;

        foreach (var child in node.Children)
        {
            if (child is EnumMemberDeclaration member)
            {
                if (member.Value is EmptyNode)
                {
                    hasUnspecifiedValues = true;
                }
                else
                {
                    hasSpecifiedValues = true;
                }
            }

            if (hasSpecifiedValues && hasUnspecifiedValues)
            {
                node.AddError("Cannot mix specified and unspecified enum values");
                return false;
            }
        }

        return true;
    }


    private TypeAttributes GetModifiers(Declaration node)
    {
        var attrs = TypeAttributes.Public;

        foreach (var modifier in node.Modifiers)
            attrs |= modifier switch
            {
                Modifier.Static => TypeAttributes.Sealed | TypeAttributes.Abstract,
                Modifier.Internal => TypeAttributes.NotPublic,
                Modifier.Public => TypeAttributes.Public,
                _ => throw new NotImplementedException()
            };

        if (node.Modifiers.Contains(Modifier.Private) || node.Modifiers.Contains(Modifier.Internal))
            attrs &= ~TypeAttributes.Public;

        return attrs | TypeAttributes.Sealed;
    }

    protected override bool ShouldListenToChildren(Driver context, EnumDeclaration node)
    {
        return false;
    }
}