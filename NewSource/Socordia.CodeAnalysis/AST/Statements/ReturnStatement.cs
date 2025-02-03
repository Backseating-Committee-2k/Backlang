namespace Socordia.CodeAnalysis.AST.Statements;

public class ReturnStatement : AstNode
{
    public ReturnStatement(AstNode expression)
    {
        Children.Add(expression);
    }

    public AstNode Value => Children.First;
}