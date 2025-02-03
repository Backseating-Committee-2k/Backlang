namespace Socordia.CodeAnalysis.AST.Statements;

public class ImportStatement : Declaration
{
    public ImportStatement(AstNode expression)
    {
        Children.Add(expression);
    }

    public AstNode Expression => Children.First;
}