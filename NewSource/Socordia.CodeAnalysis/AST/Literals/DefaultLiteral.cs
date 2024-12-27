using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Literals;

public class DefaultLiteral : AstNode
{
    public DefaultLiteral(TypeName? type)
    {
        Properties.Set(nameof(Type), type);
    }

    public TypeName? Type => Properties.GetOrDefault<TypeName>(nameof(Type));
}