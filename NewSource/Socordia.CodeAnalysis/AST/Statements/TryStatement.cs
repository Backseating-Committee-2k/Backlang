namespace Socordia.CodeAnalysis.AST.Statements;

public class TryStatement : AstNode
{
    public TryStatement(Block tryBlock, IEnumerable<CatchStatement> catches, Block? finallyBlock)
    {
        Children.Add(tryBlock);
        Children.Add(finallyBlock);
        Children.Add(catches);
    }

    public Block TryBlock => (Block)Children[0];
    public Block? FinallyBlock => (Block)Children[1];
    public IEnumerable<CatchStatement> Catches => Children.OfType<CatchStatement>();
}