namespace Socordia.CodeAnalysis.AST.Statements;

public class ImportStatement : AstNode
{
    public ImportStatement(AstNode expression)
    {
        Children.Add(expression);
    }

    public AstNode Expression => Children.First;
}