namespace Socordia.CodeAnalysis.AST.TypeNames;

public class UnitTypeName : TypeName
{
    public UnitTypeName(Identifier unit)
    {
        Children.Add(unit);
    }
    
    public Identifier Unit => (Identifier) Children[0];
}