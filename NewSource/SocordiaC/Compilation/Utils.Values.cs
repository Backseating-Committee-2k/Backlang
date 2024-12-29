using DistIL.AsmIO;
using DistIL.IR;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Expressions;
using Socordia.CodeAnalysis.AST.Literals;
using SocordiaC.Compilation.Body;

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
            _ => throw new NotImplementedException()
        };
    }
}