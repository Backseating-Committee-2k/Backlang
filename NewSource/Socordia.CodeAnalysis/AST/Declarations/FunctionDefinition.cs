namespace Socordia.CodeAnalysis.AST.Declarations;

public class FunctionDefinition : Declaration
{
    public FunctionDefinition(Signature signature, Block? body)
    {
        Properties.Set(nameof(Signature), signature);
        Children.Add(body);
    }

    public Signature Signature => Properties.GetOrThrow<Signature>(nameof(Signature))!;
    public Block? Body => (Block)Children.First;
}