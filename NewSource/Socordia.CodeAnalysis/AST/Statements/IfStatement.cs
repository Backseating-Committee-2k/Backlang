namespace Socordia.CodeAnalysis.AST.Statements;

public class IfStatement : AstNode
{
    public IfStatement(AstNode condition, Block body, Block elseBlock)
    {
        Children.Add(condition);
        Children.Add(body);
        Children.Add(elseBlock);
    }

    public AstNode Condition => Children.First;
    public Block Body => (Block)Children[1];
    public Block ElseBlock => (Block)Children.Last;
}