using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Declarations;

public class UnitDeclaration : Declaration
{
    public UnitDeclaration(string name, TypeName type)
    {
        Properties.Set(nameof(Name), name);
        Properties.Set(nameof(Type), type);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
    public TypeName Type => Properties.GetOrThrow<TypeName>(nameof(Type));
}