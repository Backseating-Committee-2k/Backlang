namespace Backlang.CodeAnalysis.AST;

public class Block : AstNode
{
    public Block(List<AstNode> body)
    {
        Children.Add(body);
    }
}