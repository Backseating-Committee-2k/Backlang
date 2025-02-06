namespace Socordia.CodeAnalysis.AST.TypeNames;

public class NoTypeName : TypeName
{
    public static readonly NoTypeName Instance { get; } = new NoTypeName();

    private NoTypeName() { }
}