namespace Socordia.CodeAnalysis.AST.Declarations.DU;

public class DiscriminatedType : AstNode
{
    public DiscriminatedType(string name, List<ParameterDeclaration> parameters)
    {
        Properties.Set(nameof(Name), name);
        Children.Add(parameters);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
}