namespace Socordia.CodeAnalysis.AST.Declarations;

public class FunctionDefinition : Declaration
{
    public FunctionDefinition(Signature signature, bool isExpressionBody, Block? body)
    {
        Children.Add(signature);
        Children.Add(body);
        Properties.Set(nameof(IsExpressionBody), isExpressionBody);
    }

    public bool IsExpressionBody => Properties.GetOrDefault<bool>(nameof(IsExpressionBody));

    public Signature Signature => (Signature)Children[0];
    public Block? Body => (Block?)Children[1];
}