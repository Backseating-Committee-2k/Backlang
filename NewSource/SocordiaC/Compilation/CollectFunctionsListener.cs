using System.Reflection;
using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;

namespace SocordiaC.Compilation;

public class CollectFunctionsListener(TypeDef type) : Listener<Driver, AstNode, FunctionDefinition>
{
    protected override void ListenToNode(Driver context, FunctionDefinition node)
    {
        var attrs = GetModifiers(node);

        var method = type.CreateMethod(node.Signature.Name.Name,
            Utils.GetTypeFromNode(node.Signature.ReturnType, type), [], attrs);

        if (type == context.FunctionsType && method.IsStatic && method.Name == "main")
        {
            context.Compilation.Module.EntryPoint = method;
        }
    }

    private MethodAttributes GetModifiers(Declaration node)
    {
        var attrs = (MethodAttributes)0;

        if (!node.HasParent)
        {
            attrs |= MethodAttributes.Static;
        }

        foreach (var modifier in node.Modifiers)
        {
            attrs |= modifier switch
            {
                Modifier.Static => MethodAttributes.Static,
                Modifier.Private => MethodAttributes.Private,
                Modifier.Protected => MethodAttributes.Family,
                Modifier.Internal => MethodAttributes.Assembly,
                Modifier.Public => MethodAttributes.Public,
                _ => throw new NotImplementedException()
            };
        }

        if (attrs == MethodAttributes.Static)
        {
            attrs |= MethodAttributes.Public;
        }

        return attrs;
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => false;
}