using Loyc;

namespace Backlang.CodeAnalysis.AST.Expressions;

public class BinaryOperator : AstNode
{
    public BinaryOperator(Symbol op, AstNode left, AstNode right)
    {
        Properties.Set(nameof(Op), op);
        Children.Add(left);
        Children.Add(right);
    }
    
    public Symbol Op => Properties.GetOrThrow<Symbol>(nameof(Op));
    public AstNode Left => Children.First;
    public AstNode Right => Children.Last;
}