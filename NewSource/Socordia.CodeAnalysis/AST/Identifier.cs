namespace Socordia.CodeAnalysis.AST;

public class Identifier : AstNode
{
    public Identifier(string name)
    {
        Properties.Set(nameof(Name), name);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
}