using System.Text;

namespace Socordia.CodeAnalysis.AST.TypeNames;

public class PointerTypeName : TypeName
{
    public PointerTypeName(TypeName type, PointerKind kind)
    {
        Children.Add(type);
        Properties.Set(nameof(Kind), kind);
    }

    public TypeName Type => (TypeName)Children[0];
    public PointerKind Kind => Properties.GetOrThrow<PointerKind>(nameof(Kind));

    public override string ToString()
    {
        var builder = new StringBuilder();

        switch (Kind)
        {
            case PointerKind.Transient:
                builder.Append('*');
                break;
            case PointerKind.Reference:
                builder.Append('&');
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        builder.Append(Type);

        return builder.ToString();
    }
}