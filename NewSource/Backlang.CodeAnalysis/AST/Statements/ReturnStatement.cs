namespace Backlang.CodeAnalysis.AST.Statements;

public class ReturnStatement : AstNode
{
    public ReturnStatement(AstNode expression)
    {
        Children.Add(expression);
    }

    public AstNode Expression => Children.First;
}