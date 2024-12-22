namespace Socordia.CodeAnalysis.AST.Expressions;

public class CollectionExpression : AstNode
{
    public CollectionExpression(List<AstNode> elements)
    {
        Children.Add(elements);
    }
}