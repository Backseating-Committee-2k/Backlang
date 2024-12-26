namespace Socordia.CodeAnalysis.AST.Expressions;

public class CallExpression : AstNode
{
    public CallExpression(Identifier callee, List<AstNode> arguments)
    {
        Children.Add(callee);
        Children.Add(arguments);
    }

    public Identifier Callee => (Identifier)Children[0];
    public IEnumerable<AstNode> Arguments => Children.Skip(1);
}