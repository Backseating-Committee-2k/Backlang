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
        var attrs = MethodAttributes.Public;

        if (!node.HasParent)
        {
            attrs |= MethodAttributes.Static;
        }

        var method = type.CreateMethod(node.Signature.Name.Name,
            Utils.GetTypeFromNode(node.Signature.ReturnType, type), [], attrs);

        if (type == context.FunctionsType && method.IsStatic && method.Name == "main")
        {
            context.Compilation.Module.EntryPoint = method;
        }
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => false;
}