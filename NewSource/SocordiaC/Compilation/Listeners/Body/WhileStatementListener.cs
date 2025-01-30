using DistIL.IR;
using DistIL.IR.Utils;
using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements.Loops;

namespace SocordiaC.Compilation.Listeners.Body;

public class WhileStatementListener : Listener<BodyCompilation, AstNode, WhileStatement>
{
    protected override void ListenToNode(BodyCompilation context, WhileStatement node)
    {
        var o2 = context.Builder.Block;

        var body = context.Builder.Method.CreateBlock();
        var condBlk = context.Builder.Method.CreateBlock();

        condBlk.SetName("cond");
        body.SetName("body");
        var after = context.Builder.Method.CreateBlock();
        after.SetName("after");
        context.Tag = new LoopBranches(o2, after); //used for break and continue

        context.Builder.SetPosition(body);
        BodyCompilation.Listener.Listen(context, node.Body);
        o2.SetBranch(condBlk);
        body.SetBranch(condBlk);

        var cond = Utils.CreateValue(node.Condition, context);
        condBlk.SetBranch(new BranchInst(cond, body, after));

        context.Builder.SetPosition(after);
        context.Tag = null;
    }
}