using DistIL.AsmIO;
using DistIL.IR;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;

namespace SocordiaC.Compilation.Listeners.Body;

public class ThrowStatementListener : Listener<BodyCompilation, AstNode, ThrowStatement>
{
    protected override void ListenToNode(BodyCompilation context, ThrowStatement node)
    {
        var value = Utils.CreateValue(node.Expression, context);

        if (value.ResultType == PrimType.String)
        {
            var ctor = context.Driver.KnownTypes.ExceptionType!.FindMethod(".ctor", new MethodSig(PrimType.Void, [new TypeSig(PrimType.String)]));

            value = context.Builder.CreateNewObj(ctor, value);
        }
        else
        {
            node.Expression.AddError("Only strings can be wrapped as exception");
            return;
        }

        context.Builder.Emit(new ThrowInst(value));
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, ThrowStatement node)
    {
        return false;
    }
}