using DistIL.AsmIO;
using DistIL.IR;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;
using Socordia.CodeAnalysis.AST.Literals;
using SocordiaC.Compilation.Body;
using SocordiaC.Core.Scoping;
using SocordiaC.Core.Scoping.Items;

namespace SocordiaC.Compilation;

public partial class Utils
{
    private static Value CreateTypeOf(TypeOfExpression typeOf, BodyCompilation compilation)
    {
        var type = GetTypeFromNode(typeOf.Type, compilation.Driver.Compilation.Module)!;

        var ldToken = compilation.Builder.Emit(new CilIntrinsic.LoadHandle(compilation.Method.Module.Resolver, type));

        var typeType = compilation.Builder.Resolver.Import(typeof(Type));
        var fromTokenMethod = typeType.FindMethod("GetTypeFromHandle")!;

        return new CallInst(fromTokenMethod, [ldToken]);
    }

    private static Value CreateCall(CallExpression call, BodyCompilation compilation)
    {
        var listener = new CallExpressionListener(false);
        listener.Listen(compilation, call);

        return listener.CallInstruction;
    }

    private static Value CreateDefault(DefaultLiteral def, BodyCompilation compilation)
    {
        var type = GetTypeFromNode(def.Type, compilation.Driver.Compilation.Module)!;

        if (type.TryGetOperator("default", out var method)) return compilation.Builder.CreateCall(method!);

        return compilation.Builder.CreateDefaultOf(type);
    }

    private static Value CreateLiteral(object literalValue)
    {
        return literalValue switch
        {
            bool b => ConstInt.Create(PrimType.Bool, b ? 1 : 0),
            byte by => ConstInt.CreateI(by),
            short s => ConstInt.CreateI(s),
            int i => ConstInt.CreateI(i),
            long l => ConstInt.CreateL(l),
            float f => ConstFloat.CreateD(f),
            double d => ConstFloat.CreateD(d),
            string str => ConstString.Create(str),
            char c => ConstInt.CreateI(c),
            null => ConstNull.Create(),
            _ => throw new NotImplementedException()
        };
    }

    public static Value CreateValue(AstNode valueNode, BodyCompilation compilation)
    {
        return valueNode switch
        {
            LiteralNode literal => CreateLiteral(literal.Value),
            DefaultLiteral def => CreateDefault(def, compilation),
            CallExpression call => CreateCall(call, compilation),
            TypeOfExpression typeOf => CreateTypeOf(typeOf, compilation),
            UnaryOperatorExpression unary => CreateUnary(unary, compilation),
            BinaryOperatorExpression binary => CreateBinary(binary, compilation),
            Identifier id => ConvertIdentifier(id, compilation),
            _ => throw new NotImplementedException()
        };
    }

    private static Value ConvertIdentifier(Identifier id, BodyCompilation compilation)
    {
        if (compilation.Scope.TryGet<ScopeItem>(id.Name, out var item))
        {
            switch (item)
            {
                case VariableScopeItem var:
                    return var.Slot;
                case ParameterScopeItem p:
                    return p.Arg;
            }
        }

        id.AddError("Not found");
        return new Undef(PrimType.Void);
    }

    private static Value CreateBinary(BinaryOperatorExpression binary, BodyCompilation compilation)
    {
        var left = CreateValue(binary.Left, compilation);
        var right = CreateValue(binary.Right, compilation);

        if (left.ResultType.TryGetOperator(binary.Operator, out var method, left.ResultType, right.ResultType))
        {
            return compilation.Builder.CreateCall(method!);
        }

        return binary.Operator switch
        {
            "+" => compilation.Builder.CreateAdd(left, right),
            "-" => compilation.Builder.CreateSub(left, right),
            "*" => compilation.Builder.CreateMul(left, right),
            "/" => compilation.Builder.CreateFDiv(left, right),
            _ => throw new InvalidOperationException()
        };
    }

    private static Value CreateUnary(UnaryOperatorExpression unary, BodyCompilation compilation)
    {
        var left = CreateValue(unary.Operand, compilation);
        if (left.ResultType.TryGetOperator(unary.Operator, out var method, left.ResultType))
        {
            return compilation.Builder.CreateCall(method!);
        }

        return unary.Operator switch
        {
            "+" => left,
            "-" when left.ResultType.IsInt() && unary.Kind == UnaryOperatorKind.Prefix => compilation.Builder.Emit(new UnaryInst(UnaryOp.Neg, left)),
            "-" when left.ResultType.IsFloat() && unary.Kind == UnaryOperatorKind.Prefix => compilation.Builder.Emit(new UnaryInst(UnaryOp.FNeg, left)),
            "!" when unary.Kind == UnaryOperatorKind.Prefix => compilation.Builder.Emit(new UnaryInst(UnaryOp.Not, left)),
            "*" when unary.Kind == UnaryOperatorKind.Prefix => compilation.Builder.CreateLoad(left),
            _ => throw new InvalidOperationException()
        };
    }
}