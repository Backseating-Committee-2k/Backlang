namespace Backlang.CodeAnalysis.AST.Statements;

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