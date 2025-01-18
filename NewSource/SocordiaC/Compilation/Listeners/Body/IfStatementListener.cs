using DistIL.IR;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;

namespace SocordiaC.Compilation.Listeners.Body;

public class IfStatementListener : Listener<BodyCompilation, AstNode, IfStatement>
{
    protected override void ListenToNode(BodyCompilation context, IfStatement node)
    {
        var cond = Utils.CreateValue(node.Condition, context);

        if (!node.ElseBlock.HasChildren)
        {
            cond = NegateCondition(cond);

            context.Builder.Fork(cond, (builder, block) =>
            {
                BodyCompilation.Listener.Listen(context with
                {
                    Builder = builder,
                }, node.Body);
            });
        }
    }

    private Value NegateCondition(Value cond)
    {
        if (cond is CompareInst cmp)
        {
            cmp.Op = cmp.Op switch
            {
                CompareOp.Eq => CompareOp.Ne,
                CompareOp.Ne => CompareOp.Eq,
                CompareOp.Slt => CompareOp.Sge,
                CompareOp.Sle => CompareOp.Sgt,
                CompareOp.Sgt => CompareOp.Sle,
                CompareOp.Sge => CompareOp.Slt,
                _ => throw new NotSupportedException($"Unsupported comparison operator: {cmp.Op}")
            };
        }

        if (cond is ConstInt { BitSize: 1 } c)
        {
            return ConstInt.CreateI(c.Value == 0 ? 1 : 0);
        }

        return cond;
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, AstNode node)
    {
        return false;
    }
}