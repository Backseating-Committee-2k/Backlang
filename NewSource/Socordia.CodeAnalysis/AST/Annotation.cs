using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST;

public class Annotation : AstNode
{
    public Annotation(TypeName name, IEnumerable<AstNode> args)
    {
        Properties.Set(nameof(Name), name);
        Children.Add(args);
    }

    public TypeName Name => Properties.GetOrThrow<TypeName>(nameof(Name));
}