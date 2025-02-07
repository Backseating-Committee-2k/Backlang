using MrKWatkins.Ast;

namespace Socordia.CodeAnalysis.AST;

public class AstNode : PropertyNode<AstNode>
{
    public object? Tag { get; set; }
}