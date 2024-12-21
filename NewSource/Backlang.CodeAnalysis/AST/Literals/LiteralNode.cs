namespace Backlang.CodeAnalysis.AST.Literals;

public class LiteralNode : AstNode
{
    public LiteralNode(object? value)
    {
        Properties.Set(nameof(Value), value);
    }

    public object Value => Properties.GetOrThrow<object>(nameof(Value));
}