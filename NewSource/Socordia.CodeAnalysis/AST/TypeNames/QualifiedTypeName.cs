namespace Socordia.CodeAnalysis.AST.TypeNames;

public class QualifiedTypeName : TypeName
{
    public QualifiedTypeName(SimpleTypeName name)
    {
        Children.Add(name);

        foreach (var child in name.Children)
        {
            child.MoveTo(this);
        }
    }

    public string Namespace => string.Join('.', Children[..^1].Take(Children.Count - 1));
    public TypeName Type => (TypeName)Children.Last;

    public override string ToString() => $"{Namespace}.{Type}";
}