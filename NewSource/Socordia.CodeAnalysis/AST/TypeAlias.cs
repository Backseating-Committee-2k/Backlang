namespace Socordia.CodeAnalysis.AST;

public class TypeAlias : AstNode
{
    public TypeAlias(AstNode expr)
    {
        Children.Add(expr);
    }

    public AstNode Expression => Children[0];
}