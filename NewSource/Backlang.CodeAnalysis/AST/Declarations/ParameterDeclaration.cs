namespace Backlang.CodeAnalysis.AST.Declarations;

public class ParameterDeclaration : Declaration
{
    public ParameterDeclaration(AstNode type, string name, AstNode? defaultValue, bool assertNotNull, List<Annotation> annotations)
    {
        Properties.Set(nameof(Name), name);
        Properties.Set(nameof(DefaultValue), defaultValue);
        Properties.Set(nameof(AssertNotNull), assertNotNull);
        Properties.Set(nameof(Annotations), annotations);

        Children.Add(type);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
    public AstNode? DefaultValue => Properties.GetOrDefault<AstNode?>(nameof(DefaultValue));
    public bool AssertNotNull => Properties.GetOrDefault<bool>(nameof(AssertNotNull));

    public AstNode Type => Children[0];
}