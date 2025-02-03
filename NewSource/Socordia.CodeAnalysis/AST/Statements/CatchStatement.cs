namespace Socordia.CodeAnalysis.AST.Statements;

public class CatchStatement : AstNode
{
    public CatchStatement(Identifier exceptionType, Identifier exceptionValueName, Block body)
    {
        Children.Add(exceptionType);
        Children.Add(exceptionValueName);
        Children.Add(body);
    }

    public Identifier ExceptionType => (Identifier)Children[0];
    public Identifier ExceptionValueName => (Identifier)Children[1];
    public Block Body => (Block)Children[2];
}