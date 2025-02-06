using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Declarations;

public class EnumDeclaration : Declaration
{
    public EnumDeclaration(string name, TypeName baseType, List<EnumMemberDeclaration> members)
    {
        Properties.Set(nameof(Name), name);
        Children.Add(baseType);
        Children.Add(members);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
    public TypeName BaseType => Children.OfType<TypeName>().First();
}