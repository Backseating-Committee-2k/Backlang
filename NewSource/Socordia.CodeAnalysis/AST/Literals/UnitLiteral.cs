namespace Socordia.CodeAnalysis.AST.Literals;

public class UnitLiteral : AstNode
{
    public UnitLiteral(AstNode value, string unit)
    {
        Children.Add(value);
        Properties.Set(nameof(Unit), unit);
    }

    public AstNode Value => Children[0];
    public string Unit => Properties.GetOrThrow<string>(nameof(Unit));
}