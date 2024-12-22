using System.Reflection;
using DistIL.AsmIO;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace SocordiaC.Compilation;

public class CollectFunctionsListener : Listener<Driver, AstNode, FunctionDefinition>
{
    protected override void ListenToNode(Driver context, FunctionDefinition node)
    {
        MethodAttributes attrs = MethodAttributes.Public;

        if (!node.HasParent)
        {
            attrs |= MethodAttributes.Static;
        }

        var method = context.FunctionsType.CreateMethod(node.Signature.Name.Name,
            Utils.GetTypeFromNode(node.Signature.ReturnType), [], attrs);
    }

    protected override bool ShouldListenToChildren(Driver context, AstNode node) => false;
}

public static class Utils
{
    public static TypeDesc GetTypeFromNode(AstNode node)
    {
        if (node is SimpleTypeName id)
        {
            return id.Name switch
            {
                "none" => PrimType.Void,
                "bool" => PrimType.Bool,
                "i8" => PrimType.Byte,
                "i16" => PrimType.Int16,
                "i32" => PrimType.Int32,
                "i64" => PrimType.Int64,
                "f32" => PrimType.Single,
                "f64" => PrimType.Double,
                _ => throw new Exception("unknown type")
            };
        }

        throw new Exception("cannot get type from node");
    }
}