using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Statements;

public class VariableStatement : Declaration
{
    public VariableStatement(string name, AstNode? type, AstNode initializer, bool isMutable)
    {
        Children.Add(type);
        Children.Add(initializer);
        Properties.Set(nameof(Name), name);
        Properties.Set(nameof(IsMutable), isMutable);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
    public bool IsMutable => Properties.GetOrDefault<bool>(nameof(IsMutable));

    public TypeName? Type => (TypeName)Children.First;
    public AstNode Initializer => Children.Last;
}