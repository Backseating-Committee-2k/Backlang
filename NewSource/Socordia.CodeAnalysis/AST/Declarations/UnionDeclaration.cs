namespace Socordia.CodeAnalysis.AST.Declarations;

public class UnionDeclaration : Declaration
{
    public UnionDeclaration(string name, List<AstNode> members)
    {
        Properties.Set(nameof(Name), name);
        Children.Add(members);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
}