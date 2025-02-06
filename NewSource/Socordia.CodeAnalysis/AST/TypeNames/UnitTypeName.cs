namespace Socordia.CodeAnalysis.AST.TypeNames;

public class UnitTypeName : TypeName
{
    public UnitTypeName(SimpleTypeName unit)
    {
        Children.Add(unit);
    }

    public SimpleTypeName Unit => (SimpleTypeName)Children[0];
}