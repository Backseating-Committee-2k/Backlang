namespace Socordia.CodeAnalysis.AST.TypeNames;

public class SimpleTypeName : TypeName
{
    public SimpleTypeName(string name)
    {
        Properties.Set(nameof(Name), name);
    }

    public string Name {
        get => Properties.GetOrThrow<string>(nameof(Name));
        set => Properties.Set(nameof(Name), value);
    }

    public override string ToString()
    {
        return Name;
    }
}