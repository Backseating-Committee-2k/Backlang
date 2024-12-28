namespace Socordia.CodeAnalysis.AST;

public class Declaration : AstNode
{
    public List<Annotation> Annotations => Properties.GetOrAdd<List<Annotation>>(nameof(Annotations), _ => []);

    public DocComment? DocComment
    {
        get => Properties.GetOrThrow<DocComment>(nameof(DocComment));
        set => Properties.Set(nameof(DocComment), value);
    }

    public List<Modifier> Modifiers
    {
        get => Properties.GetOrThrow<List<Modifier>>(nameof(Modifiers));
        set => Properties.Set(nameof(Modifiers), value);
    }

    public AstNode WithAnnotations(List<Annotation> annotations)
    {
        Annotations.AddRange(annotations);
        return this;
    }
}