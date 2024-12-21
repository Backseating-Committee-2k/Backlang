namespace Backlang.CodeAnalysis.AST.Literals;

public class TupleLiteral : AstNode
{
    public TupleLiteral(List<AstNode> elements)
    {
        Children.Add(elements);
    }
}