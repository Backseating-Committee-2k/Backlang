namespace Socordia.CodeAnalysis.AST.Declarations;

public class DelegateDeclaration : Declaration
{
    public DelegateDeclaration(Signature signature)
    {
        Children.Add(signature);
    }
}