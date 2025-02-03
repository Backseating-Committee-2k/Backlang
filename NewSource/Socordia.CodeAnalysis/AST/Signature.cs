using Socordia.CodeAnalysis.AST.Declarations;
using Socordia.CodeAnalysis.AST.TypeNames;

namespace Socordia.CodeAnalysis.AST;

public class Signature : AstNode
{
    public Signature(AstNode name, AstNode? returnType, List<ParameterDeclaration> parameters, List<AstNode> generics)
    {
        Children.Add(name);
        Children.Add(returnType);
        Children.Add(parameters);
    }

    public Identifier Name => (Identifier)Children[0];
    public TypeName? ReturnType => (TypeName)Children[1];
    public IEnumerable<ParameterDeclaration> Parameters => Children.OfType<ParameterDeclaration>();
}