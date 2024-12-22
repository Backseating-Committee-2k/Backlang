namespace Socordia.CodeAnalysis.AST.Declarations;

public class ClassDeclaration : Declaration
{
    public ClassDeclaration(string name, List<AstNode> inheritances, List<AstNode> members)
    {
        Properties.Set(nameof(Name), name);
        Properties.Set(nameof(Inheritances), inheritances);
        Children.Add(members);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
    public List<AstNode> Inheritances => Properties.GetOrThrow<List<AstNode>>(nameof(Inheritances));
}