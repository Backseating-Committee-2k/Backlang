namespace Socordia.CodeAnalysis.AST;

// using t as System.Int32;
// using t as i32;
public class TypeAliasDeclaration : Declaration
{
    public TypeAliasDeclaration(AstNode expr)
    {
        Children.Add(expr);
    }

    public Identifier Name => (Identifier)Children.First.Children[0];
    public AstNode Type => Children.First.Children[1];
}