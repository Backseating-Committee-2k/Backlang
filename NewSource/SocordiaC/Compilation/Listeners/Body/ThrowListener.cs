using MrKWatkins.Ast.Listening;
using Socordia.CodeAnalysis.AST;
using Socordia.CodeAnalysis.AST.Statements;

namespace SocordiaC.Compilation.Listeners.Body;

public class ThrowStatementListener : Listener<BodyCompilation, AstNode, ThrowStatement>
{
    protected override void ListenToNode(BodyCompilation context, ThrowStatement node)
    {
        base.ListenToNode(context, node);
    }
}