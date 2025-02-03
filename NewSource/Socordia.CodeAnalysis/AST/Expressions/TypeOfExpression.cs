using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST.Expressions;

public class TypeOfExpression : AstNode
{
    public TypeOfExpression(TypeName type)
    {
        Properties.Set(nameof(Type), type);
    }

    public TypeName Type => Properties.GetOrThrow<TypeName>(nameof(Type));
}