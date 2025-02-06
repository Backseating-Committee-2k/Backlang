namespace Socordia.CodeAnalysis.AST.Statements.Loops;

public class ForStatement : AstNode
{
    public ForStatement(AstNode varExpr, AstNode type, AstNode arr, Block body)
    {
        Children.Add(varExpr);
        Children.Add(type);
        Children.Add(arr);
        Children.Add(body);
    }

    public AstNode VarExpr => Children[0];
    public AstNode Type => Children[1];
    public AstNode Arr => Children[2];
    public Block Body => (Block)Children[3];
}