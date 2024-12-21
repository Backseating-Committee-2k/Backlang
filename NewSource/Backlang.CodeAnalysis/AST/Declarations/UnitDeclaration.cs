namespace Backlang.CodeAnalysis.AST.Declarations;

public class UnitDeclaration : Declaration
{
    public UnitDeclaration(string name)
    {
        Properties.Set(nameof(Name), name);
    }

    public string Name => Properties.GetOrThrow<string>(nameof(Name));
}