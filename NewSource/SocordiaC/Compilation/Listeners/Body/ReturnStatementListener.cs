using DistIL.IR;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;

namespace SocordiaC.Compilation.Body;

public class ReturnStatementListener : Listener<BodyCompilation, AstNode, ReturnStatement>
{
    protected override void ListenToNode(BodyCompilation context, ReturnStatement node)
    {
        var value = Utils.CreateValue(node.Value, context);
        context.Builder.Emit(new ReturnInst(value));
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, AstNode node)
    {
        return false;
    }
}