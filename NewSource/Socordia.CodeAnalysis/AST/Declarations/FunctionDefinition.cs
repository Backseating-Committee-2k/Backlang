namespace Socordia.CodeAnalysis.AST.Declarations;

public class FunctionDefinition : Declaration
{
    public FunctionDefinition(Signature signature, Block? body)
    {
        Children.Add(signature);
        Children.Add(body);
    }

    public Signature Signature => (Signature)Children[0];
    public Block? Body => (Block?)Children[1];
}