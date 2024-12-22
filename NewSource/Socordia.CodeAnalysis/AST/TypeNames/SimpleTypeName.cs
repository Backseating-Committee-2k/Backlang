namespace Socordia.CodeAnalysis.AST.TypeNames;

public class SimpleTypeName : TypeName
{
    public SimpleTypeName(string name)
    {
        Properties.Set(nameof(Name), name);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));

    public override string ToString() => Name;
}