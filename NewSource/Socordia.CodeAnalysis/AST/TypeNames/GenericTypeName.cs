namespace Socordia.CodeAnalysis.AST.TypeNames;

public class GenericTypeName : SimpleTypeName
{
    public GenericTypeName(string name, List<TypeName> genericArguments) : base(name)
    {
        Properties.Set(nameof(GenericArguments), genericArguments);
    }

    public List<TypeName> GenericArguments => Properties.GetOrThrow<List<TypeName>>(nameof(GenericArguments));
}