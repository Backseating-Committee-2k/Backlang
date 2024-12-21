using Backlang.CodeAnalysis.AST.Declarations;

namespace Backlang.CodeAnalysis.AST;

public class Signature : AstNode
{
    public Signature(AstNode name, AstNode? returnType, List<ParameterDeclaration> parameters, List<AstNode> generics)
    {
        Properties.Set(nameof(Name), name);
        Children.Add(returnType);
        Children.Add(parameters);
    }

    public Identifier Name => Properties.GetOrThrow<Identifier>(nameof(Name));
    public AstNode? ReturnType => Children[0];
    public IEnumerable<ParameterDeclaration> Parameters => Children.OfType<ParameterDeclaration>();
}