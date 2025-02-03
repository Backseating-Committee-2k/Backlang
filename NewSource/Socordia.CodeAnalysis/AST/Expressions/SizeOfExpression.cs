namespace Socordia.CodeAnalysis.AST.Expressions;

public class SizeOfExpression : AstNode
{
    public SizeOfExpression(AstNode type)
    {
        Properties.Set(nameof(Type), type);
    }

    public AstNode Type => Properties.GetOrThrow<AstNode>(nameof(Type));
}