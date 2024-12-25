using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Declarations;

public class ClassDeclaration : Declaration
{
    public ClassDeclaration(string name, List<TypeName> inheritances, List<AstNode> members)
    {
        Properties.Set(nameof(Name), name);
        Properties.Set(nameof(Inheritances), inheritances);
        Children.Add(members);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
    public List<TypeName> Inheritances => Properties.GetOrThrow<List<TypeName>>(nameof(Inheritances));
}