namespace Socordia.CodeAnalysis.AST;

public class Annotation : AstNode
{
    public Annotation(string name, IEnumerable<AstNode> args)
    {
        Properties.Set(nameof(Name), name);
        Children.Add(args);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
}