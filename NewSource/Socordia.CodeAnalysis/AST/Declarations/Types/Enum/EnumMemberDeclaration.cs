namespace Socordia.CodeAnalysis.AST.Declarations;

public class EnumMemberDeclaration : Declaration
{
    public EnumMemberDeclaration(Identifier name, AstNode value)
    {
        Children.Add(name);
        Children.Add(value);
    }

    public Identifier Name => (Identifier)Children[0];
    public AstNode Value => Children[1];
}