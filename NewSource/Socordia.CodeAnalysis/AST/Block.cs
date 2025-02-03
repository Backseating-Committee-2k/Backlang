namespace Socordia.CodeAnalysis.AST;

public class Block : AstNode
{
    public Block(IEnumerable<AstNode> body)
    {
        Children.Add(body);
    }
}