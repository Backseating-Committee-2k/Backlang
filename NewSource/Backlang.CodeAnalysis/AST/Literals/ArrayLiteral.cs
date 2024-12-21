namespace Backlang.CodeAnalysis.AST.Literals;

public class ArrayLiteral : AstNode
{
    public ArrayLiteral(List<AstNode> elements)
    {
        Children.Add(elements);
    }
}