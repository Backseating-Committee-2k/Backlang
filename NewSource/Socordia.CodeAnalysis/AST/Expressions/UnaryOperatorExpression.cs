namespace Socordia.CodeAnalysis.AST.Expressions;

public class UnaryOperatorExpression : AstNode
{
    public UnaryOperatorExpression(string op, AstNode operand, UnaryOperatorKind kind)
    {
        Properties.Set(nameof(Operator), op);
        Properties.Set(nameof(Kind), kind);

        Children.Add(operand);
    }

    public string Operator => Properties.GetOrThrow<string>(nameof(Operator));
    public UnaryOperatorKind Kind => Properties.GetOrThrow<UnaryOperatorKind>(nameof(Kind));
    public AstNode Operand => Children.First;
}