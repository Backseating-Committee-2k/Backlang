namespace Socordia.CodeAnalysis.AST.Declarations;

public class RulesDeclaration : Declaration
{
    public RulesDeclaration(AstNode target, List<AstNode> rules)
    {
        Properties.Set(nameof(Target), target);

        Children.Add(rules);
    }

    public AstNode Target => Properties.GetOrThrow<AstNode>(nameof(Target));
}