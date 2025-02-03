namespace Socordia.CodeAnalysis.AST.Declarations.DU;

public class DiscriminatedUnionDeclaration : Declaration
{
    public DiscriminatedUnionDeclaration(string name, List<DiscriminatedType> types)
    {
        Properties.Set(nameof(Name), name);
        Children.Add(types);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
}