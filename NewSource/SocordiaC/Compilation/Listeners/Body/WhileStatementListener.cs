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

        context.Builder.SetPosition(body);
        BodyCompilation.Listener.Listen(context, node.Body);
        o2.SetBranch(condBlk);
        body.SetBranch(condBlk);

        var after = context.Builder.Method.CreateBlock();
        after.SetName("after");

        var cond = Utils.CreateValue(node.Condition, context);
        condBlk.SetBranch(new BranchInst(cond, body, after));

        context.Builder.SetPosition(after);

       // IRPrinter.ExportPlain(context.Builder.Method, Console.Out);
    }
}