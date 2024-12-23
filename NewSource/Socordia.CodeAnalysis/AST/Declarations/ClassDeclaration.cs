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

public class EnumDeclaration : Declaration
{
    public EnumDeclaration(string name, TypeName baseType, List<AstNode> members)
    {
        Properties.Set(nameof(Name), name);
        Properties.Set(nameof(BaseType), baseType);
        Children.Add(members);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
    public TypeName BaseType => Properties.GetOrThrow<TypeName>(nameof(BaseType));
}