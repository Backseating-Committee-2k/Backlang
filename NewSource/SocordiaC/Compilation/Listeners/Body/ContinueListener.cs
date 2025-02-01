using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements.Loops;

namespace SocordiaC.Compilation.Listeners.Body;

public class ContinueListener : Listener<BodyCompilation, AstNode, ContinueStatement>
{
    protected override void ListenToNode(BodyCompilation context, ContinueStatement node)
    {
        if (context.Tag is LoopBranches loopBranches)
        {
            context.Builder.SetBranch(loopBranches.Start);

            return;
        }

        node.AddError("Continue is only in loops allowed");
    }

    protected override bool ShouldListenToChildren(BodyCompilation context, ContinueStatement node)
    {
        return false;
    }
}