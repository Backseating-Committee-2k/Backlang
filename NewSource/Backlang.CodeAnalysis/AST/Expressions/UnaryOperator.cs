using Loyc;

namespace Backlang.CodeAnalysis.AST.Expressions;

public class UnaryOperator : AstNode
{
    public UnaryOperator(Symbol op, AstNode operand)
    {
        Properties.Set(nameof(Op), op);
        Children.Add(operand);
    }

    public Symbol Op => Properties.GetOrThrow<Symbol>(nameof(Op));
    public AstNode Operand => Children.First;
}