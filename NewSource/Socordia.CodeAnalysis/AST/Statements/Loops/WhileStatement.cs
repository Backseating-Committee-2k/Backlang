namespace Socordia.CodeAnalysis.AST.Statements.Loops;

public class WhileStatement : AstNode
{
    public WhileStatement(AstNode condition, Block body)
    {
        Children.Add(condition);
        Children.Add(body);
    }

    public AstNode Condition => Children.First;
    public Block Body => (Block)Children.Last;
}