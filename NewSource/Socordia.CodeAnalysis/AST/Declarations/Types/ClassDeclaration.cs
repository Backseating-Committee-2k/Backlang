using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Declarations;

public class ClassDeclaration : Declaration
{
    public ClassDeclaration(string name, TypeName? baseType, List<TypeName> inheritances, List<TypeMemberDeclaration> members)
    {
        Properties.Set(nameof(Name), name);
        Properties.Set(nameof(Implementations), inheritances);
        Properties.Set(nameof(BaseType), baseType);
        Children.Add(members);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));

    public TypeName? BaseType => Properties.GetOrThrow<TypeName>(nameof(BaseType))!;

    public List<TypeName> Implementations => Properties.GetOrThrow<List<TypeName>>(nameof(Implementations));
}