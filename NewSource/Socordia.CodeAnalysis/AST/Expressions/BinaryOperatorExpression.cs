using Loyc;

namespace Socordia.CodeAnalysis.AST.Expressions;

public class BinaryOperatorExpression : AstNode
{
    public BinaryOperatorExpression(string op, AstNode left, AstNode right)
    {
        Properties.Set(nameof(Operator), op);
        Children.Add(left);
        Children.Add(right);
    }
    
    public string Operator => Properties.GetOrThrow<string>(nameof(Operator));
    public AstNode Left => Children.First;
    public AstNode Right => Children.Last;
}