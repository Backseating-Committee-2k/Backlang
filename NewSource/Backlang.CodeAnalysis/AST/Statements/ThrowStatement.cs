namespace Backlang.CodeAnalysis.AST.Statements;

public class ThrowStatement : AstNode
{
    public ThrowStatement(AstNode expression)
    {
        Children.Add(expression);
    }

    public AstNode Expression => Children.First;
}