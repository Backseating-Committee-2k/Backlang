using Socordia.CodeAnalysis.Parsing.ParsePoints.Declarations;

namespace Socordia.CodeAnalysis.AST;

public class RootBlock(IEnumerable<AstNode> body) : Block(body)
{
    public Scope Scope { get; set; } = new Scope(null);
}