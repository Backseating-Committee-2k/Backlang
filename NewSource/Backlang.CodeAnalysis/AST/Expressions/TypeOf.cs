namespace Backlang.CodeAnalysis.AST.Expressions;

public class TypeOf : AstNode
{
    public TypeOf(AstNode type)
    {
        Properties.Set(nameof(Type), type);
    }

    public AstNode Type => Properties.GetOrThrow<AstNode>(nameof(Type));
}