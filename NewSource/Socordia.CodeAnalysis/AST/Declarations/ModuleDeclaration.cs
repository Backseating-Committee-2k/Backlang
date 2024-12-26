using Socordia.CodeAnalysis.AST.Expressions;

namespace Socordia.CodeAnalysis.AST.Declarations;

public class ModuleDeclaration : Declaration
{
    public ModuleDeclaration(AstNode name)
    {
        Children.Add(name);
    }

    public AstNode Name => Children[0];

    public string? Canonicalize()
    {
        var names = GetNames(Name);

        if (!names.Any())
        {
            return null;
        }

        return string.Join('.', names);
    }

    private IEnumerable<string> GetNames(AstNode node)
    {
        List<string> result = [];
        while (true)
        {
            if (node is Identifier id)
            {
                result.Add(id.Name);
                break;
            }
            if (node is BinaryOperator { Operator: "'." } bin)
            {
                result.Add(((Identifier)bin.Left).Name);

                node = bin.Right;
                continue;
            }

            break;
        }

        return result;
    }
}