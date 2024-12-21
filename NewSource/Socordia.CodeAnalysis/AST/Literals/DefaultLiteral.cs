namespace Socordia.CodeAnalysis.AST.Literals;

public class DefaultLiteral : AstNode
{
    public DefaultLiteral(AstNode? type)
    {
        Properties.Set(nameof(Type), type);
    }

    public AstNode? Type => Properties.GetOrDefault<AstNode>(nameof(Type));
}