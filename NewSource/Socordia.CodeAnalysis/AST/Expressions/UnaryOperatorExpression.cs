namespace Socordia.CodeAnalysis.AST.Expressions;

public class UnaryOperatorExpression : AstNode
{
    public UnaryOperatorExpression(string op, AstNode operand, UnaryOperatorKind kind)
    {
        Properties.Set(nameof(Op), op);
        Properties.Set(nameof(Kind), kind);

        Children.Add(operand);
    }

    public string Op => Properties.GetOrThrow<string>(nameof(Op));
    public UnaryOperatorKind Kind => Properties.GetOrThrow<UnaryOperatorKind>(nameof(Kind));
    public AstNode Operand => Children.First;
}