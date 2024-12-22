namespace Socordia.CodeAnalysis.AST.Statements.Loops;

public class DoWhileStatement : AstNode
{
    public DoWhileStatement(Block body, AstNode condition)
    {
        Children.Add(body);
        Children.Add(condition);
    }

    public Block Body => (Block)Children[0];
    public AstNode Condition => Children[1];
}