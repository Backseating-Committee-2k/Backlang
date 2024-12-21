namespace Backlang.CodeAnalysis.AST.Expressions;

public class SizeOf : AstNode
{
    public SizeOf(AstNode type)
    {
        Properties.Set(nameof(Type), type);
    }

    public AstNode Type => Properties.GetOrThrow<AstNode>(nameof(Type));
}