using Socordia.CodeAnalysis.AST.Expressions;

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

    public QualifiedTypeName(BinaryOperatorExpression op)
    {
        AddChildrenRecursively(op);
    }

    private void AddChildrenRecursively(BinaryOperatorExpression op)
    {
        foreach (var child in op.Children)
        {
            if (child is Identifier id)
            {
                Children.Add(new SimpleTypeName(id.Name));
            }
            else if (child is BinaryOperatorExpression binaryChild && binaryChild.Operator == ".")
            {
                AddChildrenRecursively(binaryChild);
            }
        }
    }

    public string Namespace => string.Join('.', Children[..^1].Take(Children.Count - 1));
    public TypeName Type => (TypeName)Children.Last;

    public string FullName => $"{Namespace}.{Type}";

    public override string ToString() => FullName;
}