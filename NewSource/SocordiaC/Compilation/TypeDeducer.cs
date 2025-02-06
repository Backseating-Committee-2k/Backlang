using DistIL.AsmIO;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;
using Socordia.CodeAnalysis.AST.Literals;
using Socordia.CodeAnalysis.AST.TypeNames;
using System.Collections.Immutable;

namespace SocordiaC.Compilation;

public static class TypeDeducer
{
    public static readonly ImmutableDictionary<string, PrimType> TypenameTable = new Dictionary<string, PrimType>
    {
        ["obj"] = PrimType.Object,
        ["none"] = PrimType.Void,
        ["bool"] = PrimType.Bool,
        ["u8"] = PrimType.Byte,
        ["u16"] = PrimType.UInt16,
        ["u32"] = PrimType.UInt32,
        ["u64"] = PrimType.UInt64,
        ["i8"] = PrimType.SByte,
        ["i16"] = PrimType.Int16,
        ["i32"] = PrimType.Int32,
        ["i64"] = PrimType.Int64,
        ["f32"] = PrimType.Single,
        ["f64"] = PrimType.Double,
        ["char"] = PrimType.Char,
        ["string"] = PrimType.String
    }.ToImmutableDictionary();

    public static TypeDesc? Deduce(AstNode node, ModuleResolver resolver)
    {
        if (node is SimpleTypeName simpleTypeName)
        {
            if (TypenameTable.TryGetValue(simpleTypeName.Name, out var deduce))
            {
                return deduce;
            }
        }

        if (node is LiteralNode literal)
        {
            return resolver.Import(literal.Value.GetType());
        }

        if (node is UnaryOperatorExpression unary)
        {
            return DeduceUnary(unary, resolver);
        }

        if (node is TypeOfExpression)
        {
            return resolver.SysTypes.Type;
        }

        if (node is DefaultLiteral defaultLiteral)
        {
            return TypenameTable[defaultLiteral.Type!.ToString()];
        }

        node.AddError("Cannot deduce type");
        return null;
    }

    private static TypeDesc? DeduceUnary(UnaryOperatorExpression unary, ModuleResolver resolver)
    {
        var left = Deduce(unary.Operand, resolver);

        if (left.TryGetOperator(unary.Operator, out var opMethod, left))
        {
            return opMethod.ReturnType;
        }

        if (unary.Operator == "&")
        {
            return left.CreatePointer();
        }

        if (unary.Operator == "*")
        {
            return left.ElemType;
        }

        if (unary.Operator == "!")
        {
            return PrimType.Bool;
        }

        if (unary.Operator == "-")
        {
            return left.ElemType;
        }

        return null;
    }
}