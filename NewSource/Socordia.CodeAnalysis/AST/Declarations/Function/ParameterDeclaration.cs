using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Declarations;

public class ParameterDeclaration : Declaration
{
    public ParameterDeclaration(AstNode type, string name, AstNode? defaultValue, bool assertNotNull,
        List<Annotation> annotations, bool isOut)
    {
        Properties.Set(nameof(Name), name);
        Properties.Set(nameof(DefaultValue), defaultValue);
        Properties.Set(nameof(AssertNotNull), assertNotNull);
        Properties.Set(nameof(Annotations), annotations);
        Properties.Set(nameof(IsOut), isOut);

        Children.Add(type);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
    public AstNode? DefaultValue => Properties.GetOrDefault<AstNode?>(nameof(DefaultValue));
    public bool AssertNotNull => Properties.GetOrDefault<bool>(nameof(AssertNotNull));

    public bool IsOut => Properties.GetOrDefault<bool>(nameof(IsOut));

    public TypeName Type => (TypeName)Children[0];
}