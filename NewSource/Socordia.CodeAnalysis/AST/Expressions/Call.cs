namespace Socordia.CodeAnalysis.AST.Expressions;

public class Call : AstNode
{
    public Call(AstNode callee, List<AstNode> arguments)
    {
        Children.Add(callee);
        Children.Add(arguments);
    }

    public AstNode Callee => Children[0];
    public IEnumerable<AstNode> Arguments => Children.Skip(1);
}